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
            var state = this.State;
            if (state.Id <= 0)
            {
                var dbResult = await _scheduleRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
                var result = this.ObjectMapper.Map<ScheduleTaskState>(dbResult);
                return result;
            }
            return null;
        }

        public async Task<ApiResult> AddAsync(ScheduleTaskDTO model)
        {
            //参数校验
            if (!model.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            //转换为数据库实体
            ScheduleTaskDO scheduleDO = this.ObjectMapper.Map<ScheduleTaskDO>(model);

            scheduleDO.CreateBy = model.UserId;
            scheduleDO.CreateDate = System.DateTime.Now;
            scheduleDO.ModityBy = model.UserId;
            scheduleDO.ModityDate = System.DateTime.Now;

            bool bRet = await _scheduleRepository.AddAsync(scheduleDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }

            //更新服务状态
            ScheduleTaskState scheduleTaskState = this.ObjectMapper.Map<ScheduleTaskState>(scheduleDO);
            await this.Persist(ProcessAction.Create, scheduleTaskState);
            return ApiResultUtil.IsSuccess(model.Id.ToString());
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
            return await Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> UpdateAsync(ScheduleTaskDTO model)
        {
            if (model.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }

            bool bRet = await _scheduleRepository.SetAsync(() => new { task_name = model.TaskName, line_code = model.LineCode, version = (model.Version + 1) }, oo => oo.Id == this.GrainId);

            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            ScheduleTaskState scheduleTaskState = this.State;
            scheduleTaskState.TaskName = model.TaskName;
            scheduleTaskState.LineCode = model.LineCode;
            scheduleTaskState.Version++;

            ///任务完成 进行系统对接
            if (model.TaskState == TaskState.Complete)
            {
                ScheduleTaskVO scheduleTaskVO = this.ObjectMapper.Map<ScheduleTaskVO>(this.State);
                await Observer.PublishAsync(EventContract.TaskInterface, scheduleTaskVO);
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
