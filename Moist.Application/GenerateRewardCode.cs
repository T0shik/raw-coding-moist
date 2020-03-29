using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;

namespace Moist.Application
{
    public class GenerateRewardCode
    {
        private readonly IShopStore _store;
        private readonly ICodeStore _codeStore;

        public GenerateRewardCode(
            IShopStore store,
            ICodeStore codeStore)
        {
            _store = store;
            _codeStore = codeStore;
        }

        public async Task<string> Create(string userId, int shopId, int schemaId)
        {
            if (!await _store.UserCanGenerateSchemaCode(userId, shopId))
            {
                //todo return decent error.
                throw new Exception("error");
            }

            var code = await _codeStore.CreateSchemaRewardCode(shopId, schemaId);

            return code;
        }
    }
}