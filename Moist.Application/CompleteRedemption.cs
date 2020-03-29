using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Exceptions;

namespace Moist.Application
{
    public class CompleteRedemption
    {
        private readonly IShopStore _shopStore;
        private readonly IUserManager _userManager;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public CompleteRedemption(
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

        public async Task<bool> Complete(int shopId, string code)
        {
            var result = await _codeStore.ValidateRedemptionCode(shopId, code);
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