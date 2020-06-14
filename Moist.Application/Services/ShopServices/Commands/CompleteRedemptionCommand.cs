using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.DateTimeInfrastructure;

namespace Moist.Application.Services.ShopServices.Commands
{
    public class CompleteRedemptionCommand : IRequest<Response<Empty>>
    {
        public int ShopId { get; set; }
        public string Code { get; set; }
    }

    public class CompleteRedemptionCommandHandler : IRequestHandler<CompleteRedemptionCommand, Response<Empty>>
    {
        private readonly IUserStore _userStore;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public CompleteRedemptionCommandHandler(
            IUserStore userStore,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _userStore = userStore;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<Response<Empty>> Handle(CompleteRedemptionCommand request, CancellationToken cancellationToken)
        {
            var result = await _codeStore.ValidateRedemptionCode(request.ShopId, request.Code);
            if (!result.Success)
            {
                return Response.Fail("Invalid Code");
            }

            var progress = await _userStore.GetProgressAsync(result.UserId, result.ProgressId);

            progress.Completed = true;
            progress.CompletedOn = _dateTime.Now;

            return Response.Ok("Code Valid");
        }
    }
}