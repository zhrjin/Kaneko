using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using MSDemo.Grains.Service;
using Kaneko.Hosts;
using MSDemo.IGrains;

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
                      .ConfigureLogging(builder =>
                      {
                      })
                      .ConfigureServices(services =>
                      {
                          services.Configure<ConsoleLifetimeOptions>(options =>
                          {
                              options.SuppressStatusMessages = true;
                          });
                      })
                      .AddOrleans("MSDemo", typeof(TestGrain).Assembly, typeof(HealthCheckInfo).Assembly);
    }
}
