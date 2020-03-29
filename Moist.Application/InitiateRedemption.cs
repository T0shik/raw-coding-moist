using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Application
{
    public class InitiateRedemption
    {
        private readonly IUserManager _userManager;
        private readonly IShopStore _shopStore;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public InitiateRedemption(
            IUserManager userManager,
            IShopStore shopStore,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _userManager = userManager;
            _shopStore = shopStore;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<string> Redeem(string customer, int progressId)
        {
            var progress = await _userManager.GetProgressAsync(customer, progressId);

            var schema = await _shopStore.GetSchema(progress.SchemaId);
            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.ReachedGoal(progress.Progress))
            {
                throw new Exception();
            }

            if (!assignedSchema.Valid(_dateTime))
            {
                throw new Exception();
            }

            return await _codeStore.CreateRedemptionCode();
        }
    }
}