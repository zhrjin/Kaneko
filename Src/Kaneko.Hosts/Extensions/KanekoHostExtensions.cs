using Kaneko.Consul;
using Kaneko.Server.Orleans.Grains.HealthCheck;
using Kaneko.Hosts.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using Kaneko.Consul.Controller;
using Kaneko.Hosts.Controller;
using Kaneko.Hosts.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using Kaneko.Hosts.Validator;

namespace Kaneko.Hosts.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class KanekoHostExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="controllerAssembly"></param>
        /// <param name="configuration"></param>
        /// <param name="corsConfig"></param>
        /// <param name="mvcConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddKaneko(this IServiceCollection services, Assembly controllerAssembly, IConfiguration configuration, Action<CorsOptions> corsConfig = null, Action<MvcOptions> mvcConfig = null)
        {
            if (bool.Parse(configuration["Consul:Enable"]))
            {
                services.AddConsul(configuration);
            }

            if (corsConfig != null)
            {
                services.AddCors(corsConfig);
            }

            //orleans健康检查
            //services
            //     .AddHostedService<HealthCheckHostedService>()
            //     .Configure<HealthCheckHostedServiceOptions>(options =>
            //     {
            //         options.Port = int.Parse(configuration["Orleans:HealthCheckPort"]);
            //         options.PathString = "/health";
            //     });

            services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(mvcConfig)
                    .AddApplicationPart(controllerAssembly)
                    .AddApplicationPart(typeof(KaneKoController).Assembly)
                    .AddApplicationPart(typeof(ConsulController).Assembly)
                    ;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = configuration["ServiceName"], Version = "v1" });

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

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKaneko(this IApplicationBuilder app, IConfiguration configuration)
        {
            string serviceName = configuration["ServiceName"];
            if (bool.Parse(configuration["Consul:Enable"]))
            {
                app.UseConsul();
            }
            app.UseCors(serviceName);
            app.UseSwagger(c =>
            {
                //加上服务名，支持直接在ocelot进行api测试
                string basepath = "";
                if (!string.IsNullOrEmpty(serviceName))
                {
                    basepath = $"/{serviceName}";
                }
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    OpenApiPaths paths = new OpenApiPaths();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(basepath + path.Key, path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", serviceName);
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            return app;
        }
    }
}
