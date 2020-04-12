using System;
using System.Collections;
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

        public Task<int> GetUsersShopId(string userId)
        {
            return Db.Employees
                     .Where(x => x.UserId == userId)
                     .Select(x => x.ShopId)
                     .FirstOrDefaultAsync();
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
            var storeId = await GetUsersShopId(userId);

            var schemas = await Db.Schemas
                                  .Where(x => x.Id == storeId)
                                  .ToListAsync();

            return schemas;
        }

        public Task<bool> SaveSchema(Schema schema)
        {
            Db.Schemas.Add(schema);
            return Save();
        }

        public Task<bool> CreateShopForUser(string userId)
        {
            var shop = new Shop();
            shop.Employees.Add(new Employee
            {
                UserId = userId,
                CanChangeProfile = true,
                CanActivateSchema = true,
                CanGenerateSchemaCode = true,
            });

            Db.Shops.Add(shop);
            return Save();
        }

        public Task<Shop> GetProfile(int shopId)
        {
            return Db.Shops.FirstOrDefaultAsync(x => x.Id == shopId);
        }

        public Task<T> GetProfile<T>(string userId, Expression<Func<Shop, T>> selector)
        {
            return Db.Employees
                     .Include(x => x.Shop)
                     .Where(x => x.UserId == userId)
                     .Select(x => x.Shop)
                     .Select(selector)
                     .FirstOrDefaultAsync();
        }

        public Task<bool> UserCanGenerateSchemaCode(string userId, int storeId)
        {
            return Task.FromResult(true);
            return Db.Employees
                     .Where(x => x.UserId == userId && x.ShopId == storeId)
                     .Select(x => x.CanGenerateSchemaCode)
                     .FirstOrDefaultAsync();
        }

        public Task<bool> UserCanChangeProfile(string userId, int storeId)
        {
            return Db.Employees
                     .Where(x => x.UserId == userId && x.ShopId == storeId)
                     .Select(x => x.CanChangeProfile)
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