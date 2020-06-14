using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Application.Services.Users.Commands
{
    public class JoinSchemaCommand : UserRequest, IRequest<Response<SchemaProgress>>
    {
        public int SchemaId { get; set; }
    }

    public class JoinSchemaCommandHandler : IRequestHandler<JoinSchemaCommand, Response<SchemaProgress>>
    {
        private readonly IShopStore _shopStore;
        private readonly IUserStore _userStore;
        private readonly IDateTime _dateTime;

        public JoinSchemaCommandHandler(
            IShopStore shopStore,
            IUserStore userStore,
            IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userStore = userStore;
            _dateTime = dateTime;
        }

        public async Task<Response<SchemaProgress>> Handle(
            JoinSchemaCommand request,
            CancellationToken cancellationToken)
        {
            var schema = await _shopStore.GetSchema(request.SchemaId);

            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
            {
                return Response.Fail<SchemaProgress>("Schema not valid");
            }

            if (await _userStore.InSchemaAsync(request.UserId, request.SchemaId))
            {
                return Response.Fail<SchemaProgress>("Schema already joined");
            }

            var progress = await _userStore.CreateSchemaProgressAsync(request.UserId, request.SchemaId);
            return Response.Ok(progress, "Schema Joined");
        }
    }
}