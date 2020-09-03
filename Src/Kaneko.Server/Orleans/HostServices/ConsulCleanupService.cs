using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.HostServices
{
    public class ConsulCleanupService : IHostedService
    {
        private readonly ILocalSiloDetails localSiloDetails;
        private readonly IMembershipTable membershipTable;

        public ConsulCleanupService(ILocalSiloDetails localSiloDetails, IMembershipTable membershipTable)
        {
            this.localSiloDetails = localSiloDetails;
            this.membershipTable = membershipTable;
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.membershipTable.UpdateIAmAlive(new MembershipEntry
            {
                SiloAddress = this.localSiloDetails.SiloAddress,
                IAmAliveTime = DateTime.UtcNow.AddMinutes(-10),
                Status = SiloStatus.Dead, // This isn't used to determine if a silo is defunct but I'm going to set it anyway.
            });

            await this.membershipTable.CleanupDefunctSiloEntries(DateTimeOffset.UtcNow.AddMinutes(-5));
        }
    }
}
