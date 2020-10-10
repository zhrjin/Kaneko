﻿using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.TaskManager.Domain;

namespace YTSoft.CC.IGrains.TaskManager
{
    /// <summary>
    /// 任务管理(有状态)
    /// </summary>
    public interface IScheduleTaskStateGrain : IGrainWithIntegerKey, IStateGrain<ScheduleTaskState>
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
        Task<ApiResult> AddAsync(SubmitDTO<ScheduleTaskDTO> model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<ScheduleTaskDTO> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync();

        Task<ApiResult> CapWithTransaction();
    }
}