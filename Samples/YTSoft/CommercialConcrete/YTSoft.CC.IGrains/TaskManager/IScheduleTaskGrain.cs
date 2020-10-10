using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.TaskManager.Domain;

namespace YTSoft.CC.IGrains.TaskManager
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