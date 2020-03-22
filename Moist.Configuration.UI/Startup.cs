using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moist.Core;
using Moist.Database;
using Moist.Database.Stores;

namespace Moist.Configuration.UI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<AppDbContext>(options =>
            {
                if (_env.IsDevelopment())
                {
                    options.UseInMemoryDatabase("DEV");
                }
                else
                {
                    options.UseInMemoryDatabase("DEV");
                }
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        if (_env.IsDevelopment())
                        {
                            options.Password.RequireDigit = false;
                            options.Password.RequiredLength = 1;
                            options.Password.RequireLowercase = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequireNonAlphanumeric = false;
                            options.Password.RequiredUniqueChars = 1;
                        }
                    })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IShopStore, ShopStore>();
            services.AddScoped<CreateSchemaContext>();
            services.AddScoped<ChangeShopProfileContext>();
            services.AddScoped<InitialiseShopContext>();

            services.AddControllersWithViews();

            var mvcBuilder = services.AddRazorPages();

            if (_env.IsDevelopment())
                mvcBuilder.AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}