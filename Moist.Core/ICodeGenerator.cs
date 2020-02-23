using System.Threading.Tasks;

namespace Moist.Core {
    public interface ICodeGenerator
    {
        Task<string> CreateRedemptionCode();
        Task<bool> ValidateCode(string code);
    }
}