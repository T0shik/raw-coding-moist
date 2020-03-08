using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Configuration.Forms {
    public class CloseSchemaContext
    {
        private readonly IShopStore _shopStore;

        public CloseSchemaContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<bool> Close(int shopId, int schemaId)
        {
            var shop = await _shopStore.GetSchema<BaseSchema>(schemaId);
            
            if (shop.ShopId != shopId)
            {
                return false;
            }
            shop.Enabled = false;

            return true;
        }
    }
}