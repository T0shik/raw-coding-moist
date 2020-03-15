using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core
{
    //todo write implementations for this!
    public interface IShopStore : IStore
    {
        Task<T> GetSchema<T>(int schemaId);
        Task<bool> SaveDaysVisitedSchema(DaysVisitedSchemaSchema form);
        Task<bool> CreateShopForUser(string userId);
        Task<Shop> GetProfile(int shopId);
        Task<T> GetProfile<T>(string userId, Expression<Func<Shop, T>> selector);
        Task<bool> UserCanChangeProfile(string userId, int storeId);
    }
}