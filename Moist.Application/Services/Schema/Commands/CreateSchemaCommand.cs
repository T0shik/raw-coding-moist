using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Schema.Commands
{
    public class CreateSchemaCommand : IRequest<Response>
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SchemaType Type { get; set; }
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class CreateSchemaCommandHandler : IRequestHandler<CreateSchemaCommand, Response>
    {
        private readonly IShopStore _shopStore;

        public CreateSchemaCommandHandler(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<Response> Handle(CreateSchemaCommand request, CancellationToken cancellationToken)
        {
            var shopId = await _shopStore.GetUsersShopId(request.UserId);

            bool saved = false;
            if (request.Type == SchemaType.DaysVisited)
            {
                saved =  await _shopStore.SaveSchema(new Core.Models.Schema
                {
                    ShopId = shopId,
                    Title = request.Title,
                    Description = request.Description,
                    Perpetual = request.Perpetual,
                    Goal = request.Goal,
                    ValidSince = request.ValidSince,
                    ValidUntil = request.ValidUntil
                });
            }

            return saved
                ? Response.Ok("Schema Created")
                : Response.Fail("Failed to create schema");
        }
    }
}