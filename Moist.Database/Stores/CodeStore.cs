using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Code;
using Moist.Core.Models;

namespace Moist.Database.Stores
{
    public class CodeStore : BaseStore, ICodeStore
    {
        public CodeStore(AppDbContext ctx)
            : base(ctx) { }

        public async Task<string> CreateSchemaRewardCode(int shopId, int schemaId)
        {
            var code = new Code {ShopId = shopId, SchemaId = schemaId,};

            Db.Codes.Add(code);
            await Save();

            return code.Id;
        }

        public Task<string> CreateRedemptionCode()
        {
            throw new System.NotImplementedException();
        }

        public Task<RedemptionValidationResult> ValidateRedemptionCode(int shopId, string code)
        {
            throw new System.NotImplementedException();
        }

        public Task<ValidationResult> ValidateRewardCode(int shopId, string code)
        {
            throw new System.NotImplementedException();
        }
    }
}