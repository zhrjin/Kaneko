using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.Grains
{
    public interface IStateGrain<TState> : IMainGrain where TState : IState
    {
        /// <summary>
        /// 重置状态数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult> ReinstantiateState(TState state = default);

        /// <summary>
        /// 获取状态值
        /// </summary>
        /// <returns></returns>
        Task<TState> GetState();
    }
}
