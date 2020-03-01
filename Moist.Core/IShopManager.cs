using System.Threading.Tasks;

namespace Moist.Core {
    public interface IShopManager
    {
        Task<T> GetSchema<T>(int shopId);
    }
}