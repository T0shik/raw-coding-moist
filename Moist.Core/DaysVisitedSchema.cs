using System;
using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core
{
    public class DaysVisitedSchema
    {
        private readonly IUserManager _userManager;
        private readonly IShopManager _shopManager;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IDateTime _dateTime;

        public DaysVisitedSchema(
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

        public void Reward()
        {
            Counter++;
        }

        public int Counter { get; set; }

        public async Task<string> InitiateRedemption(string customer, int progressId)
        {
            var progress = await _userManager.GetProgressAsync(customer, progressId);

            var config = await _shopManager.GetSchema<DaysVisitedSchemaConfiguration>(progress.SchemaId);

            if (progress.Progress < config.Goal)
            {
                throw new Exception();
            }

            if (!config.Perpetual)
            {
                var now = _dateTime.Now;
                if (now < config.ValidSince
                    || now > config.ValidUntil)
                {
                    throw new Exception();
                }
            }

            return await _codeGenerator.CreateRedemptionCode();
        }

        public Task<bool> CompleteRedemption(string code)
        {
            return _codeGenerator.ValidateCode(code);
        }
    }
}