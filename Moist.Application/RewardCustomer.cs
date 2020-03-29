using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Application
{
    public class RewardCustomer
    {
        private readonly IShopStore _shopStore;
        private readonly IUserManager _userManager;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public RewardCustomer(
            IShopStore shopStore,
            IUserManager userManager,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userManager = userManager;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<bool> Reward(string customerId, int progressId, string code)
        {
            var progress = await _userManager.GetProgressAsync(customerId, progressId);

            var schema = await _shopStore.GetSchema(progress.SchemaId);
            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
            {
                throw new Exception();
            }

            var result = await _codeStore.ValidateRewardCode(schema.ShopId, code);
            if (!result.Success)
            {
                throw new Exception();
            }

            progress.Progress++;
            return true;
        }
    }
}