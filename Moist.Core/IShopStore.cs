using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core
{
    public interface IShopStore : IStore
    {
        IAsyncEnumerable<T> GetShops<T>(Expression<Func<Shop, T>> selector);

        Task<Schema> GetSchema(int schemaId);
        Task<List<Schema>> GetSchemas(string userId);
        Task<bool> SaveSchema(Schema schema);
        Task<bool> UserCanGenerateSchemaCode(string userId, int storeId);
        Task<bool> UserCanActivateSchema(string userId, int storeId);
    }
}