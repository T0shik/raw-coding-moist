using System.Threading;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Database;

namespace Moist.Application.Services.ShopServices.Commands
{
    public class CreateShopProfileCommand : UserRequest, ICommand<Shop>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateShopProfileCommandHandler : IHandler<CreateShopProfileCommand, Shop>
    {
        private readonly AppDbContext _ctx;

        public CreateShopProfileCommandHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response<Shop>> Handle(CreateShopProfileCommand request, CancellationToken cancellationToken)
        {
            var shopId = await _ctx.GetUsersShopId(request.UserId);

            if (shopId > 0)
            {
                return Response.Fail<Shop>("Shop already initialised");
            }

            var shop = new Shop
            {
                Name = request.Name,
                Description = request.Description,
            };

            shop.Employees.Add(new Employee
            {
                UserId = request.UserId,
                CanChangeProfile = true,
                CanActivateSchema = true,
                CanGenerateSchemaCode = true,
            });

            _ctx.Shops.Add(shop);

            await _ctx.SaveChangesAsync(cancellationToken);

            return Response.Ok(shop, "Shop Initialised");
        }
    }
}