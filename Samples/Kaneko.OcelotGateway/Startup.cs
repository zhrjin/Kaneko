using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using Microsoft.OpenApi.Models;
using IdentityServer4.AccessTokenValidation;
using System;

namespace Kaneko.OcelotGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ocelotOptions = new OcelotOptions();
            Configuration.Bind(ocelotOptions);

            //注册ID4校验方式1
            //services.AddAuthentication("Bearer")
            //        .AddJwtBearer(authenticationProviderKey, options =>
            //        {
            //            options.Authority = "http://192.168.0.106:12345";
            //            options.RequireHttpsMetadata = false;
            //            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //            {
            //                ValidateAudience = false
            //            };
            //        });

            foreach (var route in ocelotOptions.Routes)
            {
                if (route.AuthenticationOptions == null || string.IsNullOrEmpty(route.AuthenticationOptions.AuthenticationProviderKey)) { continue; }

                //注册ID4校验方式2
                services.AddAuthentication()
                        .AddIdentityServerAuthentication(route.AuthenticationOptions.AuthenticationProviderKey, option =>
                        {
                            option.Authority = ocelotOptions.KanekoIdentityCenter.Authority;
                            option.RequireHttpsMetadata = false;
                            option.SupportedTokens = SupportedTokens.Both;
                            option.EnableCaching = ocelotOptions.KanekoIdentityCenter.EnableCaching;
                            option.CacheDuration = TimeSpan.FromMinutes(ocelotOptions.KanekoIdentityCenter.CacheDurationMinutes);
                        })
                       ;
            }

            services.AddOcelot().AddConsul().AddPolly().AddCacheManager(x => x.WithDictionaryHandle());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new OpenApiInfo { Title = "Kaneko.ApiGateway", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var ocelotOptions = new OcelotOptions();
            Configuration.Bind(ocelotOptions);

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var route in ocelotOptions.Routes)
                {
                    if (string.IsNullOrEmpty(route.ServiceName) || string.IsNullOrWhiteSpace(route.DownstreamPathTemplate)
                    || route.DownstreamPathTemplate.ToLower().IndexOf("swagger") == -1)
                    { continue; }

                    options.SwaggerEndpoint($"{route.ServiceName}/swagger/v1/swagger.json", route.ServiceName);
                    options.RoutePrefix = string.Empty;
                }
            });

            app.UseOcelot().Wait();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
