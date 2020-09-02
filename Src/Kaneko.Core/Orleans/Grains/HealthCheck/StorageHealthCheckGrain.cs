using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Placement;
using Orleans.Runtime;

namespace Kaneko.Core.Orleans.Grains.HealthCheck
{
    [PreferLocalPlacement]
    public class StorageHealthCheckGrain : Grain, IStorageHealthCheckGrain
    {
        private readonly IPersistentState<StorageHealthTestState> state;

        public StorageHealthCheckGrain([PersistentState("State")] IPersistentState<StorageHealthTestState> state)
        {
            this.state = state;
        }

        public async Task CheckAsync()
        {
            try
            {
                state.State.Guid = Guid.NewGuid();
                await state.WriteStateAsync();
                await state.ReadStateAsync();
                await state.ClearStateAsync();
            }
            finally
            {
                DeactivateOnIdle();
            }
        }
    }

    public interface IStorageHealthCheckGrain : IGrainWithGuidKey
    {
        Task CheckAsync();
    }

    public class StorageHealthTestState
    {
        public Guid Guid { set; get; }
    }
}
