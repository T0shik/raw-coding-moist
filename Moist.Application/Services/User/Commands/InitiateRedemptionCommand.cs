using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Schemas;

namespace Moist.Application.Services.User.Commands
{
    public class InitiateRedemptionCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public int SchemaId { get; set; }
    }

    public class InitiateRedemptionCommandHandler : IRequestHandler<InitiateRedemptionCommand, Response<string>>
    {
        private readonly IUserStore _userStore;
        private readonly IShopStore _shopStore;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public InitiateRedemptionCommandHandler(
            IUserStore userStore,
            IShopStore shopStore,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _userStore = userStore;
            _shopStore = shopStore;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<Response<string>> Handle(InitiateRedemptionCommand request, CancellationToken cancellationToken)
        {
            var progress = await _userStore.GetProgressAsync(request.UserId, request.SchemaId);

            var schema = await _shopStore.GetSchema(progress.SchemaId);
            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
            {
                return Response.Fail<string>(null,  "Schema not valid");
            }

            if (!assignedSchema.ReachedGoal(progress.Progress))
            {
                return Response.Fail<string>(null,  "Goal not reached");
            }

            var code = await _codeStore.CreateRedemptionCode();
            return Response.Ok(code, "Code Created");
        }
    }
}