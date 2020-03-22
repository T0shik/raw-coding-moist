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
        private readonly IShopStore _shopStore;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public InitiateRedemption(
            IUserManager userManager,
            IShopStore shopStore,
            ICodeGenerator codeGenerator,
            IDateTime dateTime)
        {
            _userManager = userManager;
            _shopStore = shopStore;
            _codeGenerator = codeGenerator;
            _dateTime = dateTime;
        }

        public async Task<string> Redeem(string customer, int progressId)
        {
            var progress = await _userManager.GetProgressAsync(customer, progressId);

            var config = await _shopStore.GetSchema(progress.SchemaId);

            // if (progress.Progress < config.Goal)
            // {
            //     throw new Exception();
            // }
            //
            // if (!config.Active(_dateTime))
            // {
            //     throw new Exception();
            // }

            return await _codeGenerator.CreateRedemptionCode();
        }
    }
}