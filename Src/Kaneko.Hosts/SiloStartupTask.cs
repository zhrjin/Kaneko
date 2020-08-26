using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kaneko.Hosts
{
    public class SiloStartupTask : IStartupTask
    {
        readonly IServiceProvider serviceProvider;
        public SiloStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task Execute(CancellationToken cancellationToken)
        {
            await Startup.StartupTasks(serviceProvider);
        }
    }
}
