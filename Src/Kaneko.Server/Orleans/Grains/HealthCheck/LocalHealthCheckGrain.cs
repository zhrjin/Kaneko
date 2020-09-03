using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace Kaneko.Server.Orleans.Grains.HealthCheck
{
    [StatelessWorker(1)]
    public class LocalHealthCheckGrain : Grain, ILocalHealthCheckGrain
    {
        public Task PingAsync() => Task.CompletedTask;
    }

    public interface ILocalHealthCheckGrain : IGrainWithGuidKey
    {
        Task PingAsync();
    }
}
