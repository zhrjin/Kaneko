using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Topshelf;
using YTSoft.BasicData.Hosts.Core;

namespace YTSoft.BasicData.Hosts
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

            string sSiloPort = configuration["Orleans:SiloNetworkingPort"];
            string sServiceName = configuration["ServiceName"] + "_" + sSiloPort;

            HostFactory.Run(x =>
            {
                x.SetDescription("A巴基础数据服务");
                x.SetDisplayName(sServiceName);
                x.SetServiceName(sServiceName);

                x.EnableServiceRecovery(s => {
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
