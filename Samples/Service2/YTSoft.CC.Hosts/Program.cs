using Kaneko.Hosts.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YTSoft.CC.Grains.Service;
using YTSoft.CC.IGrains.Service;

namespace YTSoft.CC.Hosts
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                      .ConfigureAppConfiguration((hostContext, configBuilder) =>
                      {
                        string envName=  hostContext.HostingEnvironment.EnvironmentName;
                          configBuilder.SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                      })
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>().UseKestrel().UseIISIntegration();
                      })
                      //.ConfigureLogging((hostingContext, logging) =>
                      //{
                      //    logging.SetMinimumLevel(LogLevel.Information);
                      //    logging.AddConsole(options => options.IncludeScopes = true);
                      //})
                      .ConfigureLogging((context, logger) =>
                      {
                          logger.AddLog4Net();
                          logger.AddConsole(options => options.IncludeScopes = true);
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
