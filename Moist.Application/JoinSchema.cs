using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Application {
    public class JoinSchema
    {
        private readonly IShopManager _shopManager;
        private readonly IUserManager _userManager;
        private readonly IDateTime _dateTime;

        public JoinSchema(IShopManager shopManager,
                          IUserManager userManager,
                          IDateTime dateTime)
        {
            _shopManager = shopManager;
            _userManager = userManager;
            _dateTime = dateTime;
        }

        public async Task<SchemaProgress> Join(string customerId, int schemaId)
        {
            var schema = await _shopManager.GetSchema<DaysVisitedSchemaConfiguration>(schemaId);

            if (!schema.Active(_dateTime))
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