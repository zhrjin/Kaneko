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

            if (ocelotOptions.Redis.Enable)
            {
                //认证服务缓存
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = $"{ocelotOptions.Redis.HostName}:{ocelotOptions.Redis.Port}";
                    options.InstanceName = ocelotOptions.Redis.InstanceName;
                    if (!string.IsNullOrEmpty(ocelotOptions.Redis.Password))
                    {
                        options.ConfigurationOptions.Password = ocelotOptions.Redis.Password;
                    }
                });
            }

            foreach (var route in ocelotOptions.Routes)
            {
                if (route.AuthenticationOptions == null || string.IsNullOrEmpty(route.AuthenticationOptions.AuthenticationProviderKey)) { continue; }

                //注册ID4校验
                services.AddAuthentication()
                        .AddIdentityServerAuthentication(route.AuthenticationOptions.AuthenticationProviderKey,
                            jwtBearerOptions =>
                            {
                                jwtBearerOptions.Authority = ocelotOptions.KanekoIdentityCenter.Authority;
                                jwtBearerOptions.RequireHttpsMetadata = false;
                                jwtBearerOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                {
                                    ValidateAudience = false
                                };
                            },
                            OAuth2IntrospectionOptions =>
                            {
                                OAuth2IntrospectionOptions.Authority = ocelotOptions.KanekoIdentityCenter.Authority;
                                OAuth2IntrospectionOptions.ClientId = ocelotOptions.KanekoIdentityCenter.ClientId;
                                OAuth2IntrospectionOptions.ClientSecret = ocelotOptions.KanekoIdentityCenter.ClientSecret;
                                OAuth2IntrospectionOptions.DiscoveryPolicy.RequireHttps = false;
                                OAuth2IntrospectionOptions.EnableCaching = ocelotOptions.KanekoIdentityCenter.EnableCaching;//需要实现IDistributedCache
                                OAuth2IntrospectionOptions.CacheDuration = TimeSpan.FromMinutes(ocelotOptions.KanekoIdentityCenter.CacheDurationMinutes);
                            }
                        )
                       ;
            }

            //需要放在最上面
            services.AddCors(options =>
            {
                string[] origins = ocelotOptions.AllowedOrigins.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );
            });

            services.AddOcelot().AddConsul().AddPolly().AddCacheManager(x => x.WithDictionaryHandle());

            //services.AddButterfly(option =>
            //{
            //    //this is the url that the butterfly collector server is running on...
            //    option.CollectorUrl = ocelotOptions.TracingUrls;
            //    option.Service = "KanekoOcelot";
            //});

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

            //需要放在最上面
            app.UseCors("CorsPolicy");

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
