using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.Models.Enums;

namespace Moist.Application.Services.Schemas.Commands
{
    public class ActivateSchemaCommand : UserRequest, IRequest<Response<Empty>>
    {
        public int ShopId  { get; set; }
        public int SchemaId  { get; set; }
    }

    public class ActivateSchemaCommandHandler : IRequestHandler<ActivateSchemaCommand, Response<Empty>>
    {
        private readonly IShopStore _store;

        public ActivateSchemaCommandHandler(IShopStore store)
        {
            _store = store;
        }
        public async Task<Response<Empty>> Handle(ActivateSchemaCommand request, CancellationToken cancellationToken)
        {
            if (!await _store.UserCanActivateSchema(request.UserId, request.ShopId))
            {
                return Response.Fail("Not Allowed to activate schema");
            }

            var schema = await _store.GetSchema(request.SchemaId);
            schema.State = SchemaState.Active;
            await _store.Save();

            // todo send some notifications to interested users

            return Response.Ok("Schema Activated");
        }
    }
}