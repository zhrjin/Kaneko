using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using YTSoft.CC.Grains.XsLadeBaseManager.Repository;
using YTSoft.CC.IGrains.XsLadeBaseManager;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.Grains.XsLadeBaseManager
{
    [Reentrant]
    public class XsLadeBaseStateGrain : StateGrain<string, XsLadeBaseState>, IXsLadeBaseStateGrain
    {
        private readonly IXsLadeBaseRepository _xsLadeBaseRepository;

        public XsLadeBaseStateGrain(IXsLadeBaseRepository scheduleTaskRepository)
        {
            this._xsLadeBaseRepository = scheduleTaskRepository;
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
        protected override async Task<XsLadeBaseState> OnReadFromDbAsync()
        {
            var dbResult = await _xsLadeBaseRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsLadeBaseState>(dbResult);
            return result;
        }

        public Task<ApiResult<XsLadeBaseVO>> GetAsync()
        {
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<XsLadeBaseVO>(state);
            return Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<XsLadeBaseDTO> model)
        {
            var dto = model.Data;
            //转换为数据库实体
            XsLadeBaseDO xsLadeBaseDO = this.ObjectMapper.Map<XsLadeBaseDO>(dto);
            xsLadeBaseDO.XLB_Status = "1";
            xsLadeBaseDO.CreateBy = model.UserId;
            xsLadeBaseDO.CreateByName = model.UserName;
            xsLadeBaseDO.CreateDate = System.DateTime.Now;
            xsLadeBaseDO.ModityBy = model.UserId;
            xsLadeBaseDO.ModityByName = model.UserName;
            xsLadeBaseDO.ModityDate = System.DateTime.Now;

            bool bRet = await _xsLadeBaseRepository.AddAsync(xsLadeBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }
            //string sql = _xsLadeBaseRepository.ExecuteScript;

            //更新服务状态
            XsLadeBaseState xsLadeBaseState = this.ObjectMapper.Map<XsLadeBaseState>(xsLadeBaseDO);

            await this.Persist(ProcessAction.Create, xsLadeBaseState);

            return ApiResultUtil.IsSuccess(dto.Id.ToString());
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<XsLadeBaseDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            XsLadeBaseState xsLadeBaseState = this.State;
            if (dto.IsDel != 1)
            {
                xsLadeBaseState.XLB_LadeId = dto.XLB_LadeId;
                xsLadeBaseState.XLB_SetDate = dto.XLB_SetDate;
                xsLadeBaseState.XLB_Area = dto.XLB_Area;
                xsLadeBaseState.XLB_Origin = dto.XLB_Origin;
                xsLadeBaseState.XLB_Line = dto.XLB_Line;
                xsLadeBaseState.XLB_Client = dto.XLB_Client;
                xsLadeBaseState.XLB_Cement = dto.XLB_Cement;
                xsLadeBaseState.XLB_Number = dto.XLB_Number;
                xsLadeBaseState.XLB_Price = dto.XLB_Price;
                xsLadeBaseState.XLB_CardPrice = dto.XLB_CardPrice;
                xsLadeBaseState.XLB_Total = dto.XLB_Total;
                xsLadeBaseState.XLB_FactNum = dto.XLB_FactNum;
                xsLadeBaseState.XLB_TurnNum = dto.XLB_TurnNum;
                xsLadeBaseState.XLB_ReturnNum = dto.XLB_ReturnNum;
                xsLadeBaseState.XLB_FactTotal = dto.XLB_FactTotal;
                xsLadeBaseState.XLB_ScaleDifNum = dto.XLB_ScaleDifNum;
                xsLadeBaseState.XLB_InvoNum = dto.XLB_InvoNum;
                xsLadeBaseState.XLB_SendArea = dto.XLB_SendArea;
                xsLadeBaseState.XLB_ApproveMan = dto.XLB_ApproveMan;
                xsLadeBaseState.XLB_CarCode = dto.XLB_CarCode;
                xsLadeBaseState.XLB_Quantity = dto.XLB_Quantity;
                xsLadeBaseState.XLB_PickMan = dto.XLB_PickMan;
                xsLadeBaseState.XLB_PrintNum = dto.XLB_PrintNum;
                xsLadeBaseState.XLB_ScaleDifID = dto.XLB_ScaleDifID;
                xsLadeBaseState.XLB_IsOut = dto.XLB_IsOut;
                xsLadeBaseState.XLB_Gather = dto.XLB_Gather;
                xsLadeBaseState.XLB_IsInvo = dto.XLB_IsInvo;
                xsLadeBaseState.XLB_Collate = dto.XLB_Collate;
                xsLadeBaseState.XLB_Firm = dto.XLB_Firm;
                xsLadeBaseState.XLB_Status = dto.XLB_Status;
                xsLadeBaseState.XLB_Remark = dto.XLB_Remark;
                xsLadeBaseState.XLB_ReturnRemark = dto.XLB_ReturnRemark;
                xsLadeBaseState.XLB_IsAgainPrint = dto.XLB_IsAgainPrint;
                xsLadeBaseState.XLB_Tranist = dto.XLB_Tranist;
                xsLadeBaseState.XLB_ColType = dto.XLB_ColType;
                xsLadeBaseState.XLB_AuditCarryTime = dto.XLB_AuditCarryTime;
                xsLadeBaseState.XLB_TaskID = dto.XLB_TaskID;
                xsLadeBaseState.XLB_BackNum = dto.XLB_BackNum;
                xsLadeBaseState.XLB_IsTransmit = dto.XLB_IsTransmit;
                xsLadeBaseState.XLB_OptName = dto.XLB_OptName;
                xsLadeBaseState.XLB_OptID = dto.XLB_OptID;
                xsLadeBaseState.XLB_CarNo = dto.XLB_CarNo;
                xsLadeBaseState.XLB_AgiId = dto.XLB_AgiId;
                xsLadeBaseState.XLB_Mortar = dto.XLB_Mortar;
                xsLadeBaseState.XLB_Method = dto.XLB_Method;
                xsLadeBaseState.XLB_ReturnPrice = dto.XLB_ReturnPrice;
                xsLadeBaseState.XLB_ReturnTotal = dto.XLB_ReturnTotal;
                xsLadeBaseState.XLB_AgentPrice = dto.XLB_AgentPrice;
                xsLadeBaseState.XLB_AgentTotal = dto.XLB_AgentTotal;
                xsLadeBaseState.XLB_RecipeNo = dto.XLB_RecipeNo;
                xsLadeBaseState.XLB_TurnType = dto.XLB_TurnType;
            }
            xsLadeBaseState.IsDel = dto.IsDel;
            xsLadeBaseState.ModityBy = model.UserId;
            xsLadeBaseState.ModityByName = model.UserName;
            xsLadeBaseState.ModityDate = System.DateTime.Now;
            xsLadeBaseState.Version = dto.Version;

            XsLadeBaseDO xsLadeBaseDO = this.ObjectMapper.Map<XsLadeBaseDO>(xsLadeBaseState);

            bool bRet = await _xsLadeBaseRepository.SetAsync(xsLadeBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, xsLadeBaseState);

            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult> DeleteAsync()
        {
            if (string.IsNullOrEmpty(this.GrainId))
            {
                return ApiResultUtil.IsFailed("主键ID不能为空！");
            }

            bool bRet = await _xsLadeBaseRepository.DeleteAsync(oo => oo.Id == this.GrainId);
            if (!bRet) { return ApiResultUtil.IsFailed("数据删除失败！"); }

            await this.Persist(ProcessAction.Delete);

            return ApiResultUtil.IsSuccess();
        }


        public async Task UpdateStatusAsync(Guid transactionId, string XLBStatus, string userId, string userName)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                return;
            }

            XsLadeBaseState xsLadeBaseState = this.State;
            xsLadeBaseState.Transactions[transactionId] = xsLadeBaseState.XLB_Status;

            xsLadeBaseState.Version++;
            xsLadeBaseState.XLB_Status = XLBStatus;
            xsLadeBaseState.ModityBy = userId;
            xsLadeBaseState.ModityByName = userName;
            xsLadeBaseState.ModityDate = System.DateTime.Now;

            XsLadeBaseDO xsLadeBaseDO = this.ObjectMapper.Map<XsLadeBaseDO>(xsLadeBaseState);

            bool bRet = await _xsLadeBaseRepository.SetAsync(xsLadeBaseDO);
            if (!bRet) { return; }

            await this.Persist(ProcessAction.Update, xsLadeBaseState);
        }

        public async Task RevertStatusAsync(Guid transactionId, string XLBStatus, string userId, string userName)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                if (State.Transactions[transactionId] == XLBStatus)
                {
                    return;
                }

                XsLadeBaseState xsLadeBaseState = this.State;
                xsLadeBaseState.Version++;
                xsLadeBaseState.XLB_Status = xsLadeBaseState.Transactions[transactionId];
                xsLadeBaseState.ModityBy = userId;
                xsLadeBaseState.ModityByName = userName;
                xsLadeBaseState.ModityDate = System.DateTime.Now;

                xsLadeBaseState.Transactions[transactionId] = XLBStatus;
                XsLadeBaseDO xsLadeBaseDO = this.ObjectMapper.Map<XsLadeBaseDO>(xsLadeBaseState);

                bool bRet = await _xsLadeBaseRepository.SetAsync(xsLadeBaseDO);
                if (!bRet) { return; }

                await this.Persist(ProcessAction.Update, xsLadeBaseState);
            }
        }
    }
}
