using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using YTSoft.CC.IGrains.Entity;
using YTSoft.CC.IGrains.VO;
using System.Threading.Tasks;
using Orleans;

namespace YTSoft.CC.IGrains.Service
{
    /// <summary>
    /// 任务管理(有状态)
    /// </summary>
    public interface IScheduleTaskStateGrain : IGrainWithIntegerKey
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<ScheduleTaskVO>> GetAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(ScheduleTaskDTO model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(ScheduleTaskDTO model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync();
    }
}