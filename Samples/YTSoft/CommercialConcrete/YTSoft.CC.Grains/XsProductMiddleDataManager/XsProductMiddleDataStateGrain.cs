using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using MongoDB.Driver.Core.Operations;
using Orleans.Concurrency;
using System.Threading.Tasks;
using YTSoft.CC.Grains.XsProductMiddleDataManager.Repository;
using YTSoft.CC.IGrains.XsProductMiddleDataManager;
using YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain;

namespace YTSoft.CC.Grains.XsProductMiddleDataManager
{
    [Reentrant]
    public class XsProductMiddleDataStateGrain : StateGrain<string, XsProductMiddleDataState>, IXsProductMiddleDataStateGrain
    {
        private readonly IXsProductMiddleDataRepository _xsProductMiddleDataRepository;

        public XsProductMiddleDataStateGrain(IXsProductMiddleDataRepository scheduleTaskRepository)
        {
            this._xsProductMiddleDataRepository = scheduleTaskRepository;
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
        protected override async Task<XsProductMiddleDataState> OnReadFromDbAsync()
        {
            var dbResult = await _xsProductMiddleDataRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsProductMiddleDataState>(dbResult);
            return result;
        }

        public Task<ApiResult<XsProductMiddleDataVO>> GetAsync()
        {
            var state = this.State;
            var scheduleVO = this.ObjectMapper.Map<XsProductMiddleDataVO>(state);
            return Task.FromResult(ApiResultUtil.IsSuccess(scheduleVO));
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<XsProductMiddleDataDTO> model)
        {
            var dto = model.Data;
            //转换为数据库实体
            XsProductMiddleDataDO xsProductMiddleDataDO = this.ObjectMapper.Map<XsProductMiddleDataDO>(dto);
            xsProductMiddleDataDO.CreateBy = model.UserId;
            xsProductMiddleDataDO.CreateByName = model.UserName;
            xsProductMiddleDataDO.CreateDate = System.DateTime.Now;
            xsProductMiddleDataDO.ModityBy = model.UserId;
            xsProductMiddleDataDO.ModityByName = model.UserName;
            xsProductMiddleDataDO.ModityDate = System.DateTime.Now;

            bool bRet = await _xsProductMiddleDataRepository.AddAsync(xsProductMiddleDataDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }
            //string sql = _xsLadeBaseRepository.ExecuteScript;

            //更新服务状态
            XsProductMiddleDataState xsProductMiddleDataState = this.ObjectMapper.Map<XsProductMiddleDataState>(xsProductMiddleDataDO);

            await this.Persist(ProcessAction.Create, xsProductMiddleDataState);

            return ApiResultUtil.IsSuccess(dto.Id.ToString());
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<XsProductMiddleDataDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            XsProductMiddleDataState xsProductMiddleDataState = this.State;
            if (dto.IsDel != 1)
            {
                xsProductMiddleDataState.XPM_EID = dto.XPM_EID;
                xsProductMiddleDataState.XPM_Tagalong = dto.XPM_Tagalong;
                xsProductMiddleDataState.XPM_Matching = dto.XPM_Matching;
                xsProductMiddleDataState.XPM_CurDate = dto.XPM_CurDate;
                xsProductMiddleDataState.XPM_JobNo = dto.XPM_JobNo;
                xsProductMiddleDataState.XPM_PropNo = dto.XPM_PropNo;
                xsProductMiddleDataState.XPM_Strength = dto.XPM_Strength;
                xsProductMiddleDataState.XPM_CustNm = dto.XPM_CustNm;
                xsProductMiddleDataState.XPM_ProjNm = dto.XPM_ProjNm;
                xsProductMiddleDataState.XPM_Location = dto.XPM_Location;
                xsProductMiddleDataState.XPM_SiteNo = dto.XPM_SiteNo;
                xsProductMiddleDataState.XPM_TechReq = dto.XPM_TechReq;
                xsProductMiddleDataState.XPM_ETel = dto.XPM_ETel;
                xsProductMiddleDataState.XPM_DeliveryMode = dto.XPM_DeliveryMode;
                xsProductMiddleDataState.XPM_TTLQntyPlanned = dto.XPM_TTLQntyPlanned;
                xsProductMiddleDataState.XPM_YSLC = dto.XPM_YSLC;
                xsProductMiddleDataState.XPM_KangShenLevel = dto.XPM_KangShenLevel;
                xsProductMiddleDataState.XPM_PBBL = dto.XPM_PBBL;
                xsProductMiddleDataState.XPM_SNBH = dto.XPM_SNBH;
                xsProductMiddleDataState.XPM_SZGG = dto.XPM_SZGG;
                xsProductMiddleDataState.XPM_TaLuoDu = dto.XPM_TaLuoDu;
                xsProductMiddleDataState.XPM_Remark = dto.XPM_Remark;
                xsProductMiddleDataState.XPM_LJFL = dto.XPM_LJFL;
                xsProductMiddleDataState.XPM_FHBH = dto.XPM_FHBH;
                xsProductMiddleDataState.XPM_Truckvol = dto.XPM_Truckvol;
                xsProductMiddleDataState.XPM_YSGJ = dto.XPM_YSGJ;
                xsProductMiddleDataState.XPM_CurTime = dto.XPM_CurTime;
                xsProductMiddleDataState.XPM_JSY = dto.XPM_JSY;
                xsProductMiddleDataState.XPM_Operator = dto.XPM_Operator;
                xsProductMiddleDataState.XPM_FHR = dto.XPM_FHR;
                xsProductMiddleDataState.XPM_ProduceLine = dto.XPM_ProduceLine;
                xsProductMiddleDataState.XPM_QYDD = dto.XPM_QYDD;
                xsProductMiddleDataState.XPM_TruckNo = dto.XPM_TruckNo;
                xsProductMiddleDataState.XPM_Driver = dto.XPM_Driver;
                xsProductMiddleDataState.XPM_TBBJ = dto.XPM_TBBJ;
                xsProductMiddleDataState.XPM_SCBJ = dto.XPM_SCBJ;
                xsProductMiddleDataState.XPM_YLa = dto.XPM_YLa;
                xsProductMiddleDataState.XPM_YLb = dto.XPM_YLb;
                xsProductMiddleDataState.XPM_SCXH = dto.XPM_SCXH;
                xsProductMiddleDataState.XPM_IsTransMit = dto.XPM_IsTransMit;
                xsProductMiddleDataState.XPM_Line = dto.XPM_Line;
                xsProductMiddleDataState.XPM_RepetBill = dto.XPM_RepetBill;
            }
            xsProductMiddleDataState.IsDel = dto.IsDel;
            xsProductMiddleDataState.ModityBy = model.UserId;
            xsProductMiddleDataState.ModityByName = model.UserName;
            xsProductMiddleDataState.ModityDate = System.DateTime.Now;
            xsProductMiddleDataState.Version = dto.Version;

            XsProductMiddleDataDO xsProductMiddleDataDO = this.ObjectMapper.Map<XsProductMiddleDataDO>(xsProductMiddleDataState);

            bool bRet = await _xsProductMiddleDataRepository.SetAsync(xsProductMiddleDataDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, xsProductMiddleDataState);

            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult> DeleteAsync()
        {
            if (string.IsNullOrEmpty(this.GrainId))
            {
                return ApiResultUtil.IsFailed("主键ID不能为空！");
            }

            bool bRet = await _xsProductMiddleDataRepository.DeleteAsync(oo => oo.Id == this.GrainId);
            if (!bRet) { return ApiResultUtil.IsFailed("数据删除失败！"); }

            await this.Persist(ProcessAction.Delete);

            return ApiResultUtil.IsSuccess();
        }
    }
}
