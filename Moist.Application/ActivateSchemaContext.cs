using System.Threading.Tasks;
using Moist.Core;

namespace Moist.Configuration
{
    public class ActivateSchemaContext
    {
        private readonly IShopStore _store;

        public ActivateSchemaContext(IShopStore store)
        {
            _store = store;
        }

        public async Task Activate(string userId, int storeId, int schemaId)
        {
            if (!await _store.UserCanActivateSchema(userId, storeId))
            {
                //todo return response
            }

            var schema = await _store.GetSchema(schemaId);
            schema.Activate = true;
            await _store.Save();

            // todo send some notifications to interested users

            // todo return response
        }
    }
}