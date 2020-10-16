using Orleans.Providers;
using Kaneko.Core.Contract;
using System.Threading.Tasks;
using System.Globalization;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 有状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [StorageProvider(ProviderName = GrainStorageKey.RedisStore)]
    public abstract class StateGrain<PrimaryKey, TState> : StateBaseGrain<PrimaryKey, TState> where TState : class, IState
    {
        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            if (this.State == null)
            {
                //State序列化异常，重新从数据库加载数据
                this.State = await OnReadFromDbAsync();
                await WriteStateAsync();
            }

            await OnActivateNextAsync();
        }
    }
}
