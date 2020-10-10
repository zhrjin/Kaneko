using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using YTSoft.CC.Grains.XsReceReceivableManager.Repository;
using YTSoft.CC.IGrains.XsReceReceivableManager;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.Grains.XsReceReceivableManager
{
    [Reentrant]
    public class XsReceReceivableStateGrain : StateGrain<string, XsReceReceivableState>, IXsReceReceivableStateGrain
    {
        private readonly IXsReceReceivableRepository _xsReceReceivableRepository;

        public XsReceReceivableStateGrain(IXsReceReceivableRepository scheduleTaskRepository)
        {
            this._xsReceReceivableRepository = scheduleTaskRepository;
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
        protected override async Task<XsReceReceivableState> OnReadFromDbAsync()
        {
            var dbResult = await _xsReceReceivableRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsReceReceivableState>(dbResult);
            return result;
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<XsReceReceivableDTO> model)
        {
            var dto = model.Data;

            //转换为数据库实体
            XsReceReceivableDO xsReceReceivableDO = this.ObjectMapper.Map<XsReceReceivableDO>(dto);

            xsReceReceivableDO.CreateBy = model.UserId;
            xsReceReceivableDO.CreateByName = model.UserName;
            xsReceReceivableDO.CreateDate = System.DateTime.Now;
            xsReceReceivableDO.ModityBy = model.UserId;
            xsReceReceivableDO.ModityByName = model.UserName;
            xsReceReceivableDO.ModityDate = System.DateTime.Now;

            bool bRet = await _xsReceReceivableRepository.AddAsync(xsReceReceivableDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }
            //string sql = _xsLadeRimpactRepository.ExecuteScript;

            //更新服务状态
            XsReceReceivableState xsLadeBaseState = this.ObjectMapper.Map<XsReceReceivableState>(xsReceReceivableDO);

            await this.Persist(ProcessAction.Create, xsLadeBaseState);

            return ApiResultUtil.IsSuccess(dto.Id.ToString());
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<XsReceReceivableDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            XsReceReceivableState xsReceReceivableState = this.State;
            if (dto.IsDel != 1)
            {

            }
            xsReceReceivableState.IsDel = dto.IsDel;
            xsReceReceivableState.ModityBy = model.UserId;
            xsReceReceivableState.ModityByName = model.UserName;
            xsReceReceivableState.ModityDate = System.DateTime.Now;
            xsReceReceivableState.Version = dto.Version;

            XsReceReceivableDO xsLadeBaseDO = this.ObjectMapper.Map<XsReceReceivableDO>(xsReceReceivableState);

            bool bRet = await _xsReceReceivableRepository.SetAsync(xsLadeBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, xsReceReceivableState);

            return ApiResultUtil.IsSuccess();
        }


        public async Task SubmitAsync(Guid transactionId, SubmitDTO<XsReceReceivableDTO> model)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                return;
            }

            var dto = model.Data;

            //转换为数据库实体
            XsReceReceivableDO xsReceReceivableDO = this.ObjectMapper.Map<XsReceReceivableDO>(dto);

            xsReceReceivableDO.CreateBy = model.UserId;
            xsReceReceivableDO.CreateByName = model.UserName;
            xsReceReceivableDO.CreateDate = System.DateTime.Now;
            xsReceReceivableDO.ModityBy = model.UserId;
            xsReceReceivableDO.ModityByName = model.UserName;
            xsReceReceivableDO.ModityDate = System.DateTime.Now;


            bool bRet = await _xsReceReceivableRepository.AddAsync(xsReceReceivableDO);
            if (!bRet) { return; }

            //更新服务状态
            XsReceReceivableState xsLadeBaseState = this.ObjectMapper.Map<XsReceReceivableState>(xsReceReceivableDO);
            xsLadeBaseState.Transactions[transactionId] = true;//执行成功赋值

            await this.Persist(ProcessAction.Create, xsLadeBaseState);
        }

        public async Task RevertAsync(Guid transactionId, int isDel, string userId, string userName)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                if (State.Transactions[transactionId] == false)
                {
                    return;
                }

                XsReceReceivableState xsReceReceivableState = this.State;
                xsReceReceivableState.IsDel = isDel;
                xsReceReceivableState.ModityBy = userId;
                xsReceReceivableState.ModityByName = userName;
                xsReceReceivableState.ModityDate = System.DateTime.Now;
                xsReceReceivableState.Version++;

                XsReceReceivableDO xsLadeBaseDO = this.ObjectMapper.Map<XsReceReceivableDO>(xsReceReceivableState);

                bool bRet = await _xsReceReceivableRepository.SetAsync(xsLadeBaseDO);

                if (!bRet) { return; }

                xsReceReceivableState.Transactions[transactionId] = false;
                await this.Persist(ProcessAction.Update, xsReceReceivableState);
            }
        }
    }
}
