using Kaneko.Core.ApiResult;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.Grains
{
    public interface IStateGrain : IMainGrain
    {
        /// <summary>
        /// 重置状态数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult> ReinstantiateState();
    }
}
