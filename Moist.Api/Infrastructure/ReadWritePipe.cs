using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moist.Database;

namespace Moist.Application.Api.Infrastructure
{
    public class ReadWritePipe<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ReadWritePipe<TRequest, TResponse>> _logger;
        private readonly AppDbContext _ctx;

        public ReadWritePipe(
            ILogger<ReadWritePipe<TRequest, TResponse>> logger,
            AppDbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            switch (request)
            {
                case IQuery<TResponse> _:
                    _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    return await next();
                case ICommand<TResponse> _:
                {
                    TResponse result = default;
                    await using (var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken))
                    {
                        try
                        {
                            result = await next();
                            await transaction.CommitAsync(cancellationToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Fail during transaction pipe");
                        }
                    }

                    if (result == null)
                    {
                        throw new Exception("");
                    }

                    return result;
                }
                default:
                    return await next();
            }
        }
    }
}