using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.Models.Enums;

namespace Moist.Application.Services.Schemas.Commands
{
    public class CloseSchemaCommand : IRequest<Response<Empty>>
    {
        public int ShopId { get; set; }
        public int SchemaId { get; set; }
    }

    public class CloseSchemaCommandHandler : IRequestHandler<CloseSchemaCommand, Response<Empty>>
    {
        private readonly IShopStore _shopStore;

        public CloseSchemaCommandHandler(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<Response<Empty>> Handle(CloseSchemaCommand request, CancellationToken cancellationToken)
        {
            var schema = await _shopStore.GetSchema(request.SchemaId);

            // todo validate if user allowed to close schema

            if (schema.ShopId != request.ShopId)
            {
                return Response.Fail("Invalid Schema");
            }

            schema.State = SchemaState.Closed;

            return Response.Ok("Schema Closed");
        }
    }
}