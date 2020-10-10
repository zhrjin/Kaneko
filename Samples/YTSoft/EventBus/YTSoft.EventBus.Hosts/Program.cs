using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Topshelf;
using YTSoft.EventBus.Hosts.Core;

namespace YTSoft.EventBus.Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostLifetime, TopshelfLifetime>();
                    services.AddHostedService<WindowHostService>();
                });

            var configBuilder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();

            string urls = configuration["Urls"]; int port = 80;
            if (!string.IsNullOrEmpty(urls))
            {
                string[] array = urls.Replace("https://", "").Replace("http://", "").Split(":", StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 2)
                {
                    port = int.Parse(array[1]);
                }
            }
            string sServiceName = configuration["ServiceName"] + "_" + port;

            HostFactory.Run(x =>
            {
                x.SetDescription("A巴订阅服务");
                x.SetDisplayName(sServiceName);
                x.SetServiceName(sServiceName);

                x.EnableServiceRecovery(s =>
                {
                    s.RestartService(1);//restart the service after 1 minute
                    s.SetResetPeriod(1);
                });

                x.Service<IHost>(s =>
                {
                    s.ConstructUsing(() => builder.Build());
                    s.WhenStarted(service =>
                    {
                        service.StartAsync();
                    });
                    s.WhenStopped(service =>
                    {
                        service.StopAsync();
                    });
                });
            });
        }
    }
}
