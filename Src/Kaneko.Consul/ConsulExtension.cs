using Consul;
using Kaneko.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Kaneko.Consul
{
    public static class ConsulExtension
    {
        /// <summary>
        /// 添加Consul功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            // 配置Consul服务注册地址
            services.Configure<KanekoOptions>(configuration);
            // 配置Consul客户端
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<KanekoOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(consulOptions.Consul.HostName))
                {
                    config.Address = new Uri($"http://{consulOptions.Consul.HostName}:{consulOptions.Consul.Port}");
                }
            }));

            return services;
        }

        /// <summary>
        /// 使用Consul
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IHostApplicationLifetime appLife = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            IOptions<KanekoOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<KanekoOptions>>();
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            // 向Consul客户端注册RestApi服务
            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                // 提供健康检查的HTTP接口
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = new Uri(address, serviceOptions.Value.Consul.HealthCheckRelativeUri).OriginalString
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = address.Port
                };

                var result = consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                // 服务应用停止后发注册RestApi服务
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }

            return app;
        }
    }
}
