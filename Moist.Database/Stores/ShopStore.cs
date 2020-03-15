using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Database.Stores
{
    public class ShopStore : BaseStore, IShopStore
    {
        public ShopStore(AppDbContext ctx)
            : base(ctx) { }

        public Task<T> GetSchema<T>(int schemaId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveDaysVisitedSchema(DaysVisitedSchemaSchema form)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CreateShopForUser(string userId)
        {
            var shop = new Shop();
            shop.Employees.Add(new Employee {UserId = userId, CanChangeProfile = true});

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

        public Task<bool> UserCanChangeProfile(string userId, int storeId)
        {
            return Db.Employees
                     .Where(x => x.UserId == userId && x.ShopId == storeId)
                     .Select(x => x.CanChangeProfile)
                     .FirstOrDefaultAsync();
        }
    }
}