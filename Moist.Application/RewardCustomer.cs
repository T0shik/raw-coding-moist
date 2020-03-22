using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;

namespace Moist.Application
{
    public class RewardCustomer
    {
        private readonly IShopStore _shopStore;
        private readonly IUserManager _userManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public RewardCustomer(
            IShopStore shopStore,
            IUserManager userManager,
            ICodeGenerator codeGenerator,
            IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userManager = userManager;
            _codeGenerator = codeGenerator;
            _dateTime = dateTime;
        }

        public async Task<bool> Reward(string customerId, int progressId, string code)
        {
            var progress = await _userManager.GetProgressAsync(customerId, progressId);

            var config = await _shopStore.GetSchema(progress.SchemaId);
            //
            // if (!config.Active(_dateTime))
            // {
            //     throw new Exception();
            // }

            var result = await _codeGenerator.ValidateRewardCode(config.ShopId, code); 
            if (!result.Success)
            {
                throw new Exception();
            }

            progress.Progress++;
            return true;
        }
    }
}