using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moist.Core;
using Moist.Database.Stores;

namespace Moist.Application.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public const string CorsAll = "AllowAll";

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMoistDatabase();

            services.AddScoped<GenerateRewardCode>();
            services.AddScoped<ICodeStore, CodeStore>();
            services.AddScoped<IShopStore, ShopStore>();

            services.AddControllers();
            if (_env.IsDevelopment())
            {
                services.AddCors(options => options.AddPolicy(CorsAll, config =>
                                                                  config.AllowAnyHeader()
                                                                        .AllowAnyMethod()
                                                                        .AllowAnyOrigin()));
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseCors(CorsAll);
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}