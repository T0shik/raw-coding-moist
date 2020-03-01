using System.Threading.Tasks;

namespace Moist.Core.Code {
    public interface ICodeGenerator
    {
        Task<string> CreateRedemptionCode();
        Task<RedemptionValidationResult> ValidateRedemptionCode(int shopId, string code);
        Task<ValidationResult> ValidateRewardCode(int shopId, string code);
    }
}