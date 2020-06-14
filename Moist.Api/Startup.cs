using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moist.Application.Api.Infrastructure;
using Moist.Database;

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
                    .AddJwtBearer("Bearer",
                                  config =>
                                  {
                                      config.Authority = "http://192.168.1.104:8004";
                                      config.Audience = "user_api";
                                      config.RequireHttpsMetadata = false;
                                  });

            if (_env.IsDevelopment())
                services.AddMoistDatabase(options => options.UseInMemoryDatabase("Database"));
            else
                services.AddMoistDatabase(
                    options => options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));


            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Response).Assembly)
                    .AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdPipe<,>))
                    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ReadWritePipe<,>));

            services.AddControllers();

            if (_env.IsDevelopment())
            {
                services.AddCors(options => options.AddPolicy(CorsAll,
                                                              config =>
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