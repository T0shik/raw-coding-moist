using System;
using System.Threading;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Core.Models.Enums;
using Moist.Database;
using SchemaType = Moist.Core.SchemaType;

namespace Moist.Application.Services.Schemas.Commands
{
    public class CreateSchemaCommand : UserRequest, ICommand<Schema>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SchemaType Type { get; set; }
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class CreateSchemaCommandHandler : IHandler<CreateSchemaCommand, Schema>
    {
        private readonly AppDbContext _ctx;

        public CreateSchemaCommandHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response<Schema>> Handle(CreateSchemaCommand request, CancellationToken cancellationToken)
        {
            var shopId = await _ctx.GetUsersShopId(request.UserId);

            if (shopId == 0)
            {
                return Response.Fail<Schema>("Failed to create schema");
            }

            var schema = new Schema
            {
                ShopId = shopId,
                Title = request.Title,
                Description = request.Description,
                Perpetual = request.Perpetual,
                State = SchemaState.Started,
                Goal = request.Goal,
                ValidSince = request.ValidSince,
                ValidUntil = request.ValidUntil
            };

            _ctx.Schemas.Add(schema);

            return Response.Ok(schema, "Schema Created");
        }
    }
}