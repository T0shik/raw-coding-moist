using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moist.Application.Services;
using Moist.Core;
using Moist.Database.Stores;

namespace Moist.Application.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public const string CorsAll = "AllowAll";

        public Startup(
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", config =>
                    {
                        config.Authority = "http://192.168.1.107:8004";
                        config.Audience = "user-api";
                        config.RequireHttpsMetadata = false;
                    });

            var connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddMoistDatabase(connectionString);

            services.AddMediatR(typeof(Response).Assembly);

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}