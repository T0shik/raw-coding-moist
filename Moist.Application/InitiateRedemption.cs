using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;

namespace Moist.Application
{
    public class InitiateRedemption
    {
        private readonly IUserManager _userManager;
        private readonly IShopManager _shopManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public InitiateRedemption(
            IUserManager userManager,
            IShopManager shopManager,
            ICodeGenerator codeGenerator,
            IDateTime dateTime)
        {
            _userManager = userManager;
            _shopManager = shopManager;
            _codeGenerator = codeGenerator;
            _dateTime = dateTime;
        }

        public async Task<string> Redeem(string customer, int progressId)
        {
            var progress = await _userManager.GetProgressAsync(customer, progressId);

            var config = await _shopManager.GetSchema<DaysVisitedSchemaConfiguration>(progress.SchemaId);

            if (progress.Progress < config.Goal)
            {
                throw new Exception();
            }

            if (!config.Active(_dateTime))
            {
                throw new Exception();
            }

            return await _codeGenerator.CreateRedemptionCode();
        }
    }
}