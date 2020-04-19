using Microsoft.EntityFrameworkCore;
using Moist.Core;
using Moist.Core.DateTimeInfrastructure;
using Moist.Database;
using Moist.Database.Stores;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMoistDatabase(this IServiceCollection @this, string connectionString)
        {
            @this.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connectionString); });
            @this.AddScoped<IShopStore, ShopStore>();
            @this.AddScoped<ICodeStore, CodeStore>();
            @this.AddScoped<IUserStore, UserStore>();
            @this.AddScoped<IDateTime, CustomDateTime>();

            return @this;
        }
    }
}