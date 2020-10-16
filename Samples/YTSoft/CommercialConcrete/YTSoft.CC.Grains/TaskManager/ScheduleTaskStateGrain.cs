using Kaneko.Core.ApiResult;
using Kaneko.Dapper.Expressions;
using Kaneko.Server.Orleans.Grains;
using System;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Kaneko.Core.Contract;
using YTSoft.CC.IGrains.TaskManager.Domain;
using YTSoft.CC.Grains.TaskManager.Repository;
using YTSoft.CC.IGrains.TaskManager;
using TYSoft.Common.Model.ComConcrete;
using TYSoft.Common.Model.EventBus;
using MongoDB.Driver;
using DotNetCore.CAP;
using Orleans.MultiClient;
using YTSoft.BasicData.IGrains.DataDictionary;
using Orleans;

namespace YTSoft.CC.Grains.TaskManager
{
    [Reentrant]
    public class ScheduleTaskStateGrain : StateGrain<long, ScheduleTaskState>, IScheduleTaskStateGrain
    {
        private readonly IScheduleTaskRepository _scheduleRepository;
        private readonly IOrleansClient _orleansClient;

        public ScheduleTaskStateGrain(IScheduleTaskRepository scheduleTaskRepository, IOrleansClient orleansClient)
        {
            this._scheduleRepository = scheduleTaskRepository;
            _orleansClient = orleansClient;
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

        public async Task<ApiResult<ScheduleTaskVO>> GetAsync()
        { 
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<ScheduleTaskVO>(state);
            return await Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<ScheduleTaskDTO> model)
        {
            var dto = model.Data;
            //参数校验
            if (!dto.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            //转换为数据库实体
            ScheduleTaskDO scheduleDO = this.ObjectMapper.Map<ScheduleTaskDO>(dto);

            scheduleDO.CreateBy = model.UserId;
            scheduleDO.CreateDate = System.DateTime.Now;
            scheduleDO.ModityBy = model.UserId;
            scheduleDO.ModityDate = System.DateTime.Now;

            bool bRet = await _scheduleRepository.AddAsync(scheduleDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }

            //更新服务状态
            ScheduleTaskState scheduleTaskState = this.ObjectMapper.Map<ScheduleTaskState>(scheduleDO);

            await this.Persist(ProcessAction.Create, scheduleTaskState);

            return ApiResultUtil.IsSuccess(dto.Id.ToString());
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
                TaskListModel scheduleTaskModel = new TaskListModel() { TaskCode = dto.TaskCode };
                await Observer.PublishAsync(EventContract.ComConcrete.TaskListToZK, scheduleTaskModel);
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

        /// <summary>
        /// 分布式事务
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult> CapWithTransaction()
        {
            var bRet = await _scheduleRepository.BeginTransactionAsync(async transaction =>
            {
                bool result = await _scheduleRepository.SetAsync(() => new { task_name = "cesces" }, oo => oo.Id == this.GrainId);

                TaskListModel scheduleTaskModel = new TaskListModel() { TaskCode = "sssss" };
                await this.PublishAsync(scheduleTaskModel);

                transaction.Commit();
                return result;
            });

            return ApiResultUtil.IsSuccess();

        }
    }
}
