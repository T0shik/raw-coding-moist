using System.Threading;
using System.Threading.Tasks;
using Moist.Database;

namespace Moist.Application.Services.ShopServices.Commands {

    public class ChangeShopProfileCommand : UserRequest, ICommand<Empty>
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ChangeShopProfileCommandHandler : IHandler<ChangeShopProfileCommand, Empty>
    {
        private readonly AppDbContext _ctx;

        public ChangeShopProfileCommandHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Response<Empty>> Handle(ChangeShopProfileCommand request, CancellationToken cancellationToken)
        {
            if (!await _ctx.UserCanChangeProfile(request.UserId, request.ShopId))
            {
                return Response.Fail("Not allowed to change shop profile");
            }

            var profile = await _ctx.GetProfile(request.ShopId);

            profile.Name = request.Name;
            profile.Description = request.Description;

            return Response.Ok("Profile updated");
        }
    }
}