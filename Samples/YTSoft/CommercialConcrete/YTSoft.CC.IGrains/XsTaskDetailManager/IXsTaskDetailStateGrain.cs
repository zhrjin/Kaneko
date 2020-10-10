using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;

namespace YTSoft.CC.IGrains.XsTaskDetailManager
{
    public interface IXsTaskDetailStateGrain : IGrainWithStringKey, IStateGrain<XsTaskDetailState>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<XsTaskDetailVO>> GetAsync();



 

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync();
    }
}
