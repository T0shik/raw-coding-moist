using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Shop.Commands
{
    public class CompleteRedemptionCommand : IRequest<Response>
    {
        public int ShopId { get; set; }
        public string Code { get; set; }
    }

    public class CompleteRedemptionCommandHandler : IRequestHandler<CompleteRedemptionCommand, Response>
    {
        private readonly IUserManager _userManager;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public CompleteRedemptionCommandHandler(
            IUserManager userManager,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _userManager = userManager;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<Response> Handle(CompleteRedemptionCommand request, CancellationToken cancellationToken)
        {
            var result = await _codeStore.ValidateRedemptionCode(request.ShopId, request.Code);
            if (!result.Success)
            {
                return Response.Fail("Invalid Code");
            }

            var progress = await _userManager.GetProgressAsync(result.UserId, result.ProgressId);

            progress.Completed = true;
            progress.CompletedOn = _dateTime.Now;

            return Response.Ok("Code Valid");
        }
    }
}