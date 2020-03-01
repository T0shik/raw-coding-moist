using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;

namespace Moist.Application
{
    public class RewardCustomer
    {
        private readonly IShopManager _shopManager;
        private readonly IUserManager _userManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public RewardCustomer(
            IShopManager shopManager,
            IUserManager userManager,
            ICodeGenerator codeGenerator,
            IDateTime dateTime)
        {
            _shopManager = shopManager;
            _userManager = userManager;
            _codeGenerator = codeGenerator;
            _dateTime = dateTime;
        }

        public async Task<bool> Reward(string customerId, int progressId, string code)
        {
            var progress = await _userManager.GetProgressAsync(customerId, progressId);

            var config = await _shopManager.GetSchema<DaysVisitedSchemaConfiguration>(progress.SchemaId);

            if (!config.Active(_dateTime))
            {
                throw new Exception();
            }

            if (!await _codeGenerator.ValidateCode(config.ShopId, code))
            {
                throw new Exception();
            }

            progress.Progress++;
            return true;
        }
    }
}