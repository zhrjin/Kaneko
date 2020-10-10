using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Server.Orleans.Grains;
using Orleans.Concurrency;
using System.Threading.Tasks;
using YTSoft.CC.Grains.HyFormulaMaterManager.Repository;
using YTSoft.CC.IGrains.HyFormulaMaterManager;
using YTSoft.CC.IGrains.HyFormulaMaterManager.Domain;

namespace YTSoft.CC.Grains.HyFormulaMaterManager
{
    /// <summary>
    /// 物料名称配置
    /// </summary>
    [Reentrant]
    public class XsLadeBaseStateGrain : StateGrain<string, HyFormulaMaterState>, IHyFormulaMaterStateGrain
    {
        private readonly IHyFormulaMaterRepository _hyFormulaMaterRepository;

        public XsLadeBaseStateGrain(IHyFormulaMaterRepository xsLadeRimpactRepository)
        {
            this._hyFormulaMaterRepository = xsLadeRimpactRepository;
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
        protected override async Task<HyFormulaMaterState> OnReadFromDbAsync()
        {
            var dbResult = await _hyFormulaMaterRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<HyFormulaMaterState>(dbResult);
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<HyFormulaMaterVO>> GetAsync()
        {
            var state = this.State;
            var xsLadeRimpactVO = this.ObjectMapper.Map<HyFormulaMaterVO>(state);

            return await Task.FromResult(ApiResultUtil.IsSuccess(xsLadeRimpactVO));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddAsync(SubmitDTO<HyFormulaMaterDTO> model)
        {
            var dto = model.Data;

            HyFormulaMaterDO hyFormulaMaterDO = this.ObjectMapper.Map<HyFormulaMaterDO>(dto);
            hyFormulaMaterDO.Create(model.UserId, model.UserName);

            bool bRet = await _hyFormulaMaterRepository.AddAsync(hyFormulaMaterDO); ;
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }

            //更新服务状态
            HyFormulaMaterState hyFormulaMaterState = this.ObjectMapper.Map<HyFormulaMaterState>(hyFormulaMaterDO);

            await this.Persist(ProcessAction.Create, hyFormulaMaterState);

            return ApiResultUtil.IsSuccess("处理成功！");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateAsync(SubmitDTO<HyFormulaMaterDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            HyFormulaMaterState xsLadeBaseState = this.State;
            xsLadeBaseState.HFM_Name = dto.HFM_Name;
            xsLadeBaseState.HFM_Cement = dto.HFM_Cement;
            xsLadeBaseState.HFM_Line = dto.HFM_Line;
            xsLadeBaseState.HFM_Field = dto.HFM_Field;
            xsLadeBaseState.HFM_FactField = dto.HFM_FactField;
            xsLadeBaseState.HFM_Firm = dto.HFM_Firm;
            xsLadeBaseState.HFM_Order = dto.HFM_Order;
            xsLadeBaseState.ModityBy = model.UserId;
            xsLadeBaseState.ModityByName = model.UserName;
            xsLadeBaseState.ModityDate = System.DateTime.Now;
            xsLadeBaseState.Version = dto.Version;

            HyFormulaMaterDO xsLadeBaseDO = this.ObjectMapper.Map<HyFormulaMaterDO>(xsLadeBaseState);

            bool bRet = await _hyFormulaMaterRepository.SetAsync(xsLadeBaseDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, xsLadeBaseState);

            return ApiResultUtil.IsSuccess();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> DelAsync(SubmitDTO<HyFormulaMaterDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            HyFormulaMaterState hyFormulaMaterState = this.State;
            hyFormulaMaterState.IsDel = 1;
            hyFormulaMaterState.ModityBy = model.UserId;
            hyFormulaMaterState.ModityByName = model.UserName;
            hyFormulaMaterState.ModityDate = System.DateTime.Now;
            hyFormulaMaterState.Version = dto.Version;
            HyFormulaMaterDO xsLadeRimpactDO = this.ObjectMapper.Map<HyFormulaMaterDO>(hyFormulaMaterState);
            bool bRet = await _hyFormulaMaterRepository.SetAsync(xsLadeRimpactDO); ;
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            await this.Persist(ProcessAction.Update, hyFormulaMaterState);

            return ApiResultUtil.IsSuccess("处理成功！");
        }
    }
}
