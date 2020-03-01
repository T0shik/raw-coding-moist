using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Exceptions;

namespace Moist.Application
{
    public class CompleteRedemption
    {
        private readonly IShopManager _shopManager;
        private readonly IUserManager _userManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public CompleteRedemption(
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

        public async Task<bool> Complete(int shopId, string code)
        {
            var result = await _codeGenerator.ValidateRedemptionCode(shopId, code);
            if (!result.Success)
            {
                throw new InvalidRedemptionCode();
            }

            var progress = await _userManager.GetProgressAsync(result.UserId, result.ProgressId);

            progress.Completed = true;
            progress.CompletedOn = _dateTime.Now;
            
            return true;
        }
    }
}