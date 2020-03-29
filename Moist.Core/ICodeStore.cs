using System.Threading.Tasks;
using Moist.Core.Code;

namespace Moist.Core {
    public interface ICodeStore : IStore
    {
        Task<string> CreateSchemaRewardCode(int shopId, int schemaId);
        Task<string> CreateRedemptionCode();
        Task<RedemptionValidationResult> ValidateRedemptionCode(int shopId, string code);
        Task<ValidationResult> ValidateRewardCode(int shopId, string code);
    }
}