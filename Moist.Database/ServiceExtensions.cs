using Microsoft.EntityFrameworkCore;
using Moist.Database;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMoistDatabase(this IServiceCollection @this, string connectionString)
        {
            @this.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connectionString); });
            return @this;
        }
    }
}