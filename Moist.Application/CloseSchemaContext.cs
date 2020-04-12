using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Configuration {
    public class CloseSchemaContext
    {
        private readonly IShopStore _shopStore;

        public CloseSchemaContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<bool> Close(int shopId, int schemaId)
        {
            var shop = await _shopStore.GetSchema(schemaId);
            
            if (shop.ShopId != shopId)
            {
                return false;
            }
            shop.Closed = true;
            shop.Activate = false;

            return true;
        }
    }
}