using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Schema.Commands
{
    public class CloseSchemaCommand : IRequest<Response>
    {
        public int ShopId { get; set; }
        public int SchemaId { get; set; }
    }

    public class CloseSchemaCommandHandler : IRequestHandler<CloseSchemaCommand, Response>
    {
        private readonly IShopStore _shopStore;

        public CloseSchemaCommandHandler(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<Response> Handle(CloseSchemaCommand request, CancellationToken cancellationToken)
        {
            var schema = await _shopStore.GetSchema(request.SchemaId);

            if (schema.ShopId != request.ShopId)
            {
                return Response.Fail("Invalid Schema");
            }

            schema.Closed = true;
            schema.Activate = false;

            return Response.Ok("Schema Closed");
        }
    }
}