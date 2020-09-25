using Kaneko.Core.ApiResult;
using YTSoft.CC.IGrains.VO;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.Entity;
using Orleans;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.Service
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public interface IScheduleTaskGrain : IGrainWithStringKey
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPage<ScheduleTaskVO>> GetPageSync(SearchDTO<ScheduleTaskDTO> model);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPageLR<ScheduleTaskVO>> GetPageLRSync(SearchDTO<ScheduleTaskDTO> model);
    }
}