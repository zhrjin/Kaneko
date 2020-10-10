using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;
using Kaneko.Hosts.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YTSoft.CC.Grains.TaskManager;
using YTSoft.CC.IGrains.TaskManager;

namespace YTSoft.CC.Hosts.Core
{
    internal class WindowHostService : IHostedService, IDisposable
    {
        private IHost _siloHost;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { await Task.FromCanceled(cancellationToken); return; }

            var builder = Host.CreateDefaultBuilder()
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

            _siloHost = builder.Build();
            await _siloHost.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _siloHost.StopAsync();
        }

        public void Dispose()
        {
            _siloHost.Dispose();
        }
    }
}
