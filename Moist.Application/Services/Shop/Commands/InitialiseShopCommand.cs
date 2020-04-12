using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Shop.Commands {
    public class InitialiseShopCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
    public class InitialiseShopCommandHandler : IRequestHandler<InitialiseShopCommand, bool>
    {
        private readonly IShopStore _shopStore;

        public InitialiseShopCommandHandler(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<bool> Handle(InitialiseShopCommand request, CancellationToken cancellationToken)
        {
            return await _shopStore.CreateShopForUser(request.UserId);
        }
    }
}