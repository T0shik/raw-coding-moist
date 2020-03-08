using System.Threading.Tasks;
using Moist.Core;

namespace Moist.Configuration {
    public class InitialiseShopContext
    {
        private readonly IShopStore _shopStore;

        public InitialiseShopContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public Task<bool> Initialise(string userId)
        {
            return _shopStore.CreateShopForUser(userId);
        }
    }
}