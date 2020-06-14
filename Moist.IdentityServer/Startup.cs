using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Moist.IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            var connectionString = _config.GetConnectionString("DefaultConnection");

            if (_env.IsDevelopment())
                services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("Database"));
            else
                services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));


            // AddIdentity registers the services
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
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
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Moist.IdentityServer.Cookie";
                config.LoginPath = "/auth/login";
                config.LogoutPath = "/auth/logout";
            });

            services.AddIdentityServer()
                    .AddAspNetIdentity<IdentityUser>()
                    .AddInMemoryApiResources(Configuration.GetApis())
                    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                    .AddInMemoryClients(Configuration.GetClients())
                    .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            if (_env.IsDevelopment())
            {
                app.UseCookiePolicy(new CookiePolicyOptions {MinimumSameSitePolicy = SameSiteMode.Lax});
            }

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}