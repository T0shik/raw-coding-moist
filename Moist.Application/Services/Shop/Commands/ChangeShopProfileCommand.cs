using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Shop.Commands {

    public class ChangeShopProfileCommand : IRequest<Response>
    {
        public string UserId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ChangeShopProfileCommandHandler : IRequestHandler<ChangeShopProfileCommand, Response>
    {
        private readonly IShopStore _shopStore;

        public ChangeShopProfileCommandHandler(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }
        public async Task<Response> Handle(ChangeShopProfileCommand request, CancellationToken cancellationToken)
        {
            if (!await _shopStore.UserCanChangeProfile(request.UserId, request.ShopId))
            {
                return Response.Fail("Not allowed to change shop profile");
            }

            var profile = await _shopStore.GetProfile(request.ShopId);

            profile.Name = request.Name;
            profile.Description = request.Description;

            await _shopStore.Save();

            return Response.Ok("Profile updated");
        }
    }
}