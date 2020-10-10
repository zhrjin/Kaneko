using Kaneko.Core.ApiResult;
using Kaneko.Dapper.Expressions;
using Kaneko.Server.Orleans.Grains;
using System;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Kaneko.Core.Contract;
using YTSoft.CC.IGrains.XsTaskBaseManager.Domain;
using YTSoft.CC.Grains.XsTaskBaseManager.Repository;
using YTSoft.CC.IGrains.XsTaskBaseManager;
using TYSoft.Common.Model.ComConcrete;
using TYSoft.Common.Model.EventBus;
using Kaneko.Core.Contract;

namespace YTSoft.CC.Grains.XsTaskBaseManager
{
    [Reentrant]
    public class XsTaskBaseStateGrain : StateGrain<string, XsTaskBaseState>, IXsTaskBaseStateGrain
    {
        private readonly IXsTaskBaseRepository _xstaskbaseRepository;

        public XsTaskBaseStateGrain(IXsTaskBaseRepository XsTaskBaseRepository)
        {
            this._xstaskbaseRepository = XsTaskBaseRepository;
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
        protected override async Task<XsTaskBaseState> OnReadFromDbAsync()
        {
            var dbResult = await _xstaskbaseRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsTaskBaseState>(dbResult);
            return result;
        }

        public Task<ApiResult<XsTaskBaseVO>> GetAsync()
        {
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<XsTaskBaseVO>(state);
            return Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<XsTaskBaseDTO> model)
        {
            var dto = model.Data;
            //转换为数据库实体
            XsTaskBaseDO XsTaskBaseDO = this.ObjectMapper.Map<XsTaskBaseDO>(dto);
            XsTaskBaseDO.CreateBy = model.UserId;
            XsTaskBaseDO.CreateByName = model.UserName;
            XsTaskBaseDO.CreateDate = System.DateTime.Now;
            XsTaskBaseDO.ModityBy = model.UserId;
            XsTaskBaseDO.ModityByName = model.UserName;
            XsTaskBaseDO.ModityDate = System.DateTime.Now;

            bool bRet = await _xstaskbaseRepository.AddAsync(XsTaskBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }
            //string sql = _XsTaskBaseRepository.ExecuteScript;

            //更新服务状态
            XsTaskBaseState XsTaskBaseState = this.ObjectMapper.Map<XsTaskBaseState>(XsTaskBaseDO);

            await this.Persist(ProcessAction.Create, XsTaskBaseState);

            return ApiResultUtil.IsSuccess(dto.Id.ToString());
        }

        public async Task<ApiResult> DeleteAsync()
        {
            if (string.IsNullOrWhiteSpace(this.GrainId))
            {
                return ApiResultUtil.IsFailed("主键ID不能为空！");
            }

            bool bRet = await _xstaskbaseRepository.DeleteAsync(oo => oo.Id == this.GrainId);
            if (!bRet) { return ApiResultUtil.IsFailed("数据删除失败！"); }

            await this.Persist(ProcessAction.Delete);

            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<XsTaskBaseDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            XsTaskBaseState XsTaskBaseState = this.State;
            if (dto.IsDel != 1)
            {
                //赋值
            }
            XsTaskBaseState.IsDel = dto.IsDel;
            XsTaskBaseState.ModityBy = model.UserId;
            XsTaskBaseState.ModityByName = model.UserName;
            XsTaskBaseState.ModityDate = System.DateTime.Now;
            XsTaskBaseState.Version = dto.Version;

            XsTaskBaseDO XsTaskBaseDO = this.ObjectMapper.Map<XsTaskBaseDO>(XsTaskBaseState);

            bool bRet = await _xstaskbaseRepository.SetAsync(XsTaskBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, XsTaskBaseState);

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
