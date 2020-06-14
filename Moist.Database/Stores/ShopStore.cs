using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moist.Core;
using Moist.Core.Models;
using Moist.Core.Schemas;

namespace Moist.Database.Stores
{
    public class ShopStore : BaseStore, IShopStore
    {
        public ShopStore(AppDbContext ctx)
            : base(ctx) { }

        public IAsyncEnumerable<T> GetShops<T>(Expression<Func<Shop, T>> selector)
        {
            return Db.Shops.Select(selector).AsAsyncEnumerable();
        }



        public Task<Schema> GetSchema(int schemaId)
        {
            return Db.Schemas.FirstOrDefaultAsync(x => x.Id == schemaId);
        }

        public Task<T> GetSchema<T>(int schemaId, Expression<Func<Schema, T>> selector) where T : BaseSchema
        {
            return Db.Schemas
                     .Where(x => x.Id == schemaId)
                     .Select(selector)
                     .FirstOrDefaultAsync();
        }

        public async Task<List<Schema>> GetSchemas(string userId)
        {
            var storeId = await Db.GetUsersShopId(userId);

            var schemas = await Db.Schemas
                                  .Where(x => x.Id == storeId)
                                  .ToListAsync();

            return schemas;
        }

        public Task<bool> SaveSchema(Schema schema)
        {
            return Save();
        }


        public Task<bool> UserCanGenerateSchemaCode(string userId, int storeId)
        {
            return Task.FromResult(true);
            return Db.Employees
                     .Where(x => x.UserId == userId && x.ShopId == storeId)
                     .Select(x => x.CanGenerateSchemaCode)
                     .FirstOrDefaultAsync();
        }

        public Task<bool> UserCanActivateSchema(string userId, int storeId)
        {
            return Db.Employees
                     .Where(x => x.UserId == userId && x.ShopId == storeId)
                     .Select(x => x.CanActivateSchema)
                     .FirstOrDefaultAsync();
        }
    }
}