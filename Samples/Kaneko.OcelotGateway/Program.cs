using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kaneko.OcelotGateway
{
    public class Program
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
                    .AddJsonFile("Ocelot.json", optional: true, reloadOnChange: true);
                })
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.SetMinimumLevel(LogLevel.Information);
                     logging.AddConsole(options => options.IncludeScopes = true);
                 })
                .ConfigureLogging((context, logger) =>
                {
                    logger.AddFilter("System", LogLevel.Warning);
                    logger.AddFilter("Microsoft", LogLevel.Warning);
                    logger.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
