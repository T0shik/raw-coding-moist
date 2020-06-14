using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moist.Core.Models;

namespace Moist.Database
{
    public static class ShopQueries
    {
        public static Task<bool> UserCanChangeProfile(this AppDbContext ctx, string userId, int storeId) =>
            ctx.Employees
               .Where(x => x.UserId == userId && x.ShopId == storeId)
               .Select(x => x.CanChangeProfile)
               .FirstOrDefaultAsync();


        public static Task<int> GetUsersShopId(this AppDbContext ctx, string userId)
        {
            return ctx.Employees
                     .Where(x => x.UserId == userId)
                     .Select(x => x.ShopId)
                     .FirstOrDefaultAsync();
        }

        public static Task<Shop> GetProfile(this AppDbContext ctx, int shopId)
        {
            return ctx.Shops.FirstOrDefaultAsync(x => x.Id == shopId);
        }

        public static Task<T> GetProfile<T>(this AppDbContext ctx, string userId, Expression<Func<Shop, T>> selector)
        {
            return ctx.Employees
                     .Include(x => x.Shop)
                     .Where(x => x.UserId == userId)
                     .Select(x => x.Shop)
                     .Select(selector)
                     .FirstOrDefaultAsync();
        }
    }
}