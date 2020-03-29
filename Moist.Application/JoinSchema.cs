using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Application {
    public class JoinSchema
    {
        private readonly IShopStore _shopStore;
        private readonly IUserManager _userManager;
        private readonly IDateTime _dateTime;

        public JoinSchema(IShopStore shopStore,
                          IUserManager userManager,
                          IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userManager = userManager;
            _dateTime = dateTime;
        }

        public async Task<SchemaProgress> Join(string customerId, int schemaId)
        {
            var schema = await _shopStore.GetSchema(schemaId);

            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
            {
                throw new Exception();
            }

            if (await _userManager.InSchemaAsync(customerId, schemaId))
            {
                throw new Exception();
            }

            return await _userManager.CreateSchemaProgressAsync(customerId, schemaId);
        }
    }
}