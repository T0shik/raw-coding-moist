using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Application.Services.User.Commands
{
    public class JoinSchemaCommand : IRequest<Response<SchemaProgress>>
    {
        public string UserId { get; set; }
        public int SchemaId { get; set; }
    }

    public class JoinSchemaCommandHandler : IRequestHandler<JoinSchemaCommand, Response<SchemaProgress>>
    {
        private readonly IShopStore _shopStore;
        private readonly IUserManager _userManager;
        private readonly IDateTime _dateTime;

        public JoinSchemaCommandHandler(
            IShopStore shopStore,
            IUserManager userManager,
            IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userManager = userManager;
            _dateTime = dateTime;
        }

        public async Task<Response<SchemaProgress>> Handle(JoinSchemaCommand request, CancellationToken cancellationToken)
        {
            var schema = await _shopStore.GetSchema(request.SchemaId);

            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
            {
                return Response.Fail<SchemaProgress>(null,  "Schema not valid");
            }

            if (await _userManager.InSchemaAsync(request.UserId, request.SchemaId))
            {
                return Response.Fail<SchemaProgress>(null,  "Schema already joined");
            }

            var progress = await _userManager.CreateSchemaProgressAsync(request.UserId, request.SchemaId);
            return Response.Ok(progress, "Schema Joined");
        }
    }
}