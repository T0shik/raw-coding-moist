using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Schema.Commands
{
    public class ActivateSchemaCommand : IRequest<Response>
    {
        public string UserId  { get; set; }
        public int ShopId  { get; set; }
        public int SchemaId  { get; set; }
    }

    public class ActivateSchemaCommandHandler : IRequestHandler<ActivateSchemaCommand, Response>
    {
        private readonly IShopStore _store;

        public ActivateSchemaCommandHandler(IShopStore store)
        {
            _store = store;
        }
        public async Task<Response> Handle(ActivateSchemaCommand request, CancellationToken cancellationToken)
        {
            if (!await _store.UserCanActivateSchema(request.UserId, request.ShopId))
            {
                return Response.Fail("Not Allowed to activate schema");
            }

            var schema = await _store.GetSchema(request.SchemaId);
            schema.Activate = true;
            await _store.Save();

            // todo send some notifications to interested users

            return Response.Ok("Schema Activated");
        }
    }
}