using Kaneko.Server.Orleans.Grains;
using Orleans.Placement;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    [PreferLocalPlacement]
    [StorageProvider(ProviderName = GrainStorageKey.RedisStore)]
    public class SagaCancellationGrain : Grain<SagaCancellationGrainState>, ISagaCancellationGrain
    {
        public async Task RequestAbort()
        {
            if (!State.AbortRequested)
            {
                State.AbortRequested = true;
                await WriteStateAsync();
            }
        }

        public Task<bool> HasAbortBeenRequested()
        {
            return Task.FromResult(State.AbortRequested);
        }
    }
}
