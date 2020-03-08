using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core {
    //todo write implementations for this!
    public interface IShopStore
    {
        Task<T> GetSchema<T>(int schemaId);
        Task<bool> SaveDaysVisitedSchema(DaysVisitedSchemaSchema form);
        Task<bool> CreateShopForUser(object userid);
        Task<Shop> GetProfile(int shopId);
        Task<bool> UserCanChangeProfile(string userId, in int storeId);
    }
}