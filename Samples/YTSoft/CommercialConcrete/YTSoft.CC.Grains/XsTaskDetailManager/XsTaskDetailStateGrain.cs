using Kaneko.Core.ApiResult;
using Kaneko.Dapper.Expressions;
using Kaneko.Server.Orleans.Grains;
using System;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Kaneko.Core.Contract;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;
using YTSoft.CC.Grains.XsTaskDetailManager.Repository;
using YTSoft.CC.IGrains.XsTaskDetailManager;
using TYSoft.Common.Model.ComConcrete;
using TYSoft.Common.Model.EventBus;

namespace YTSoft.CC.Grains.XsTaskDetailManager
{
    [Reentrant]
    public class XsTaskDetailStateGrain : StateGrain<string, XsTaskDetailState>, IXsTaskDetailStateGrain
    {
        private readonly IXsTaskDetailRepository _XsTaskDetailRepository;

        public XsTaskDetailStateGrain(IXsTaskDetailRepository XsTaskDetailRepository)
        {
            this._XsTaskDetailRepository = XsTaskDetailRepository;
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
        protected override async Task<XsTaskDetailState> OnReadFromDbAsync()
        {
            var dbResult = await _XsTaskDetailRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsTaskDetailState>(dbResult);
            return result;
        }

        public Task<ApiResult<XsTaskDetailVO>> GetAsync()
        {
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<XsTaskDetailVO>(state);
            return Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }



        public async Task<ApiResult> DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(this.GrainId))
            {
                return ApiResultUtil.IsFailed("主键ID不能为空！");
            }

            bool bRet = await _XsTaskDetailRepository.DeleteAsync(oo => oo.Id == this.GrainId);
            if (!bRet) { return ApiResultUtil.IsFailed("数据删除失败！"); }

            await this.Persist(ProcessAction.Delete);

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
