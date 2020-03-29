using Microsoft.EntityFrameworkCore;
using Moist.Database;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMoistDatabase(this IServiceCollection @this)
        {
            @this.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("DEV"); });
            return @this;
        }
    }
}