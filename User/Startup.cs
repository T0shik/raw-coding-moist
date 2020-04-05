using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace User
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => {
                        config.DefaultScheme = "Cookie";
                        config.DefaultChallengeScheme = "oidc";
                    })
                    .AddCookie("Cookie")
                    .AddOpenIdConnect("oidc", config => {
                        config.Authority = "https://localhost:5004/";
                        config.ClientId = "client_id";
                        config.ClientSecret = "client_secret";
                        config.SaveTokens = true;
                        config.ResponseType = "code";
                        config.SignedOutCallbackPath = "/Home/Index";

                        config.GetClaimsFromUserInfoEndpoint = true;

                        // configure scope
                        config.Scope.Clear();
                        config.Scope.Add("openid");
                        config.Scope.Add("user-api");
                    });

            services.AddControllersWithViews();
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

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}