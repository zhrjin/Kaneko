using Kaneko.Hosts.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Topshelf;
using YTSoft.CC.Grains.TaskManager;
using YTSoft.CC.Hosts.Core;
using YTSoft.CC.IGrains.TaskManager;

namespace YTSoft.CC.Hosts
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
                x.SetDescription("A巴商砼管理服务");
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

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                      .ConfigureAppConfiguration((hostContext, configBuilder) =>
                      {
                          string envName = hostContext.HostingEnvironment.EnvironmentName;
                          configBuilder.SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                      })
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>().UseKestrel().UseIISIntegration();
                      })
                      .ConfigureLogging((context, logger) =>
                      {
                          logger.AddLog4Net();
                      })
                      .ConfigureServices(services =>
                      {
                          services.Configure<ConsoleLifetimeOptions>(options =>
                          {
                              options.SuppressStatusMessages = true;
                          });
                      })
                      .AddOrleans(typeof(ScheduleTaskGrain).Assembly, typeof(IScheduleTaskGrain).Assembly);
    }
}
