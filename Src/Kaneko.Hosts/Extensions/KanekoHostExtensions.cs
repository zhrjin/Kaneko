using Kaneko.Core.Consul;
using Kaneko.Hosts.Controller;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace Kaneko.Hosts.Extensions
{
    public static class KanekoHostExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceName"></param>
        /// <param name="controllerAssembly"></param>
        /// <param name="configuration"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddKaneko(this IServiceCollection services, string serviceName, Assembly controllerAssembly, IConfiguration configuration, Action<CorsOptions> setupAction = null)
        {
            services.AddConsul(configuration);

            services.AddControllers()
                    .AddApplicationPart(controllerAssembly)
                    .AddApplicationPart(typeof(HealthCheckController).Assembly)
                    ;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = serviceName, Version = "v1" });
            });

            if (setupAction != null)
            {
                services.AddCors(setupAction);
            }

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKaneko(this IApplicationBuilder app, string serviceName)
        {
            app.UseConsul();
            app.UseCors(serviceName);
            app.UseSwagger(c =>
            {
                //加上服务名，支持直接在ocelot进行api测试
                string basepath = $"/{serviceName}";
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
