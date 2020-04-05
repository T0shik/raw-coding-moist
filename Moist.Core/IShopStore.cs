using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Core
{
    public interface IShopStore : IStore
    {
        Task<List<T>> GetShops<T>(Expression<Func<Shop, T>> selector);

        Task<int> GetUsersShopId(string userId);
        Task<Schema> GetSchema(int schemaId);
        Task<List<Schema>> GetSchemas(string userId);
        Task<bool> SaveSchema(Schema schema);
        Task<bool> CreateShopForUser(string userId);
        Task<Shop> GetProfile(int shopId);
        Task<T> GetProfile<T>(string userId, Expression<Func<Shop, T>> selector);
        Task<bool> UserCanGenerateSchemaCode(string userId, int storeId);
        Task<bool> UserCanChangeProfile(string userId, int storeId);
        Task<bool> UserCanActivateSchema(string userId, int storeId);
    }
}