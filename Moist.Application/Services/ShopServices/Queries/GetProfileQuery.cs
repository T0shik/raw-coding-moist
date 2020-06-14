using System.Threading;
using System.Threading.Tasks;
using Moist.Database;
using ShopModel = Moist.Core.Models.Shop;

namespace Moist.Application.Services.ShopServices.Queries
{
    public class GetProfileQuery : UserRequest, IQuery<ShopModel> { }

    public class GetProfileQueryHandler : IHandler<GetProfileQuery, ShopModel>
    {
        private readonly AppDbContext _ctx;

        public GetProfileQueryHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response<ShopModel>> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _ctx.GetProfile(request.UserId, shop => shop);
            if (profile == null)
            {
                return Response.Fail<ShopModel>("Profile Not Setup");
            }

            return Response.Ok(profile);
        }
    }
}