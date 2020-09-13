using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using YTSoft.CC.IGrains.VO;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.Entity;

namespace YTSoft.CC.IGrains.Service
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public interface IScheduleTaskGrain : IMainGrain
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPage<ScheduleTaskVO>> GetPageSync(ScheduleTaskDTO model);
    }
}