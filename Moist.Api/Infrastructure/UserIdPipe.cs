using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Moist.Application.Api.Infrastructure
{
    public class UserIdPipe<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdPipe(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is UserRequest ur)
            {
                ur.UserId = _httpContextAccessor.HttpContext.User.Claims
                                                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                                               ?.Value;
            }

            return next();
        }
    }
}