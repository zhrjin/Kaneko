using Kaneko.Core.ApiResult;
using Kaneko.Dapper.Expressions;
using Kaneko.Server.Orleans.Grains;
using System;
using System.Threading.Tasks;
using YTSoft.CC.Grains.Repository;
using YTSoft.CC.IGrains.Entity;
using YTSoft.CC.IGrains.Service;
using YTSoft.CC.IGrains.State;
using YTSoft.CC.IGrains.VO;
using Orleans.Concurrency;
using YTSoft.CC.Grains.EventBus;
using Kaneko.Core.Contract;
using System.Collections.Generic;

namespace YTSoft.CC.Grains.Service
{
    [Reentrant]
    public class ScheduleTaskStateGrain : StateGrain<long, ScheduleTaskState>, IScheduleTaskStateGrain
    {
        private readonly IScheduleTaskRepository _scheduleRepository;

        public ScheduleTaskStateGrain(IScheduleTaskRepository scheduleTaskRepository)
        {
            this._scheduleRepository = scheduleTaskRepository;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

#if DEBUG //解决开发环境Redis缓存和数据库数据不一致
            if (GrainDataState.Init == this.State.GrainDataState)
            {
                var onReadDbTask = OnReadFromDbAsync();
                if (!onReadDbTask.IsCompletedSuccessfully)
                    await onReadDbTask;
                var result = onReadDbTask.Result;
                if (result != null)
                {
                    State = result;
                    State.GrainDataState = GrainDataState.Loaded;
                    await WriteStateAsync();
                }
            }
#endif
        }

        /// <summary>
        /// 初始化状态数据
        /// </summary>
        /// <returns></returns>
        protected override async Task<ScheduleTaskState> OnReadFromDbAsync()
        {
            var dbResult = await _scheduleRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<ScheduleTaskState>(dbResult);
            return result;
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<ScheduleTaskDTO> model)
        {
            //参数校验
            if (!model.Data.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            //转换为数据库实体
            ScheduleTaskDO scheduleDO = this.ObjectMapper.Map<ScheduleTaskDO>(model.Data);

            scheduleDO.CreateBy = model.UserId;
            scheduleDO.CreateDate = System.DateTime.Now;
            scheduleDO.ModityBy = model.UserId;
            scheduleDO.ModityDate = System.DateTime.Now;

            bool bRet = await _scheduleRepository.AddAsync(scheduleDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }

            //更新服务状态
            ScheduleTaskState scheduleTaskState = this.ObjectMapper.Map<ScheduleTaskState>(scheduleDO);
            await this.Persist(ProcessAction.Create, scheduleTaskState);
            return ApiResultUtil.IsSuccess(model.Data.Id.ToString());
        }

        public async Task<ApiResult> DeleteAsync()
        {
            if (this.GrainId <= 0)
            {
                return ApiResultUtil.IsFailed("主键ID不能为空！");
            }

            bool bRet = await _scheduleRepository.DeleteAsync(oo => oo.Id == this.GrainId);
            if (!bRet) { return ApiResultUtil.IsFailed("数据删除失败！"); }

            await this.Persist(ProcessAction.Delete);

            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult<ScheduleTaskVO>> GetAsync()
        {
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<ScheduleTaskVO>(state);

            var dd = await this.GrainFactory.GetGrain<IScheduleTaskGrain>("1").GetPageSync(new SearchDTO<ScheduleTaskDTO> { PageIndex = 1, PageSize = 10 });

            return await Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<ScheduleTaskDTO> model)
        {
            var dto = model.Data;
            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }

            bool bRet = await _scheduleRepository.SetAsync(() => new { task_name = dto.TaskName, line_code = dto.LineCode, version = (dto.Version + 1) }, oo => oo.Id == this.GrainId);


            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            ScheduleTaskState scheduleTaskState = this.State;
            scheduleTaskState.TaskName = dto.TaskName;
            scheduleTaskState.LineCode = dto.LineCode;
            scheduleTaskState.Version++;

            ///任务完成 进行系统对接
            if (dto.TaskState == TaskState.Complete)
            {
                ScheduleTaskVO scheduleTaskVO = this.ObjectMapper.Map<ScheduleTaskVO>(this.State);
                await PublishAsync(scheduleTaskVO);
            }
            await this.Persist(ProcessAction.Update, scheduleTaskState);
            return ApiResultUtil.IsSuccess();

        }

        /// <summary>
        /// 异常处理
        /// </summary>
        protected override Func<Exception, Task> FuncExceptionHandler => (exception) =>
        {
            return Task.CompletedTask;
        };
    }
}
