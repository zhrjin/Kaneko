using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using MSDemo.Grains.Service;
using Microsoft.Extensions.Logging;
using Kaneko.Hosts.Extensions;
using MSDemo.IGrains.Service;

namespace MSDemo
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
                          configBuilder.SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                      })
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>();
                      })
                      .ConfigureLogging((hostingContext, logging) =>
                      {
                          //logging.AddFilter("Orleans", LogLevel.Trace);
                          //logging.AddFilter("Orleans", LogLevel.Warning);
                          //logging.AddFilter("Orleans", LogLevel.Debug);
                          //logging.AddConsole();
                          //logging.AddDebug();
                          //logging.AddEventSourceLogger();
                          logging.SetMinimumLevel(LogLevel.Information);
                          logging.AddConsole(options => options.IncludeScopes = true);
                      })
                      .ConfigureServices(services =>
                      {
                          services.Configure<ConsoleLifetimeOptions>(options =>
                          {
                              options.SuppressStatusMessages = true;
                          });
                      })
                      .AddOrleans(nameof(MSDemo), typeof(TestGrain).Assembly, typeof(ITestGrain).Assembly);
    }
}
