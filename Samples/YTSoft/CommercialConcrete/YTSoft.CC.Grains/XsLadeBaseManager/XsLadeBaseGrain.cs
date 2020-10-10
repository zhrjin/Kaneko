using Dapper;
using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Kaneko.Server.Orleans.Services;
using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbBasicFirmManager;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;
using YTSoft.BasicData.IGrains.XsCompyBaseManager;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;
using YTSoft.CC.Grains.XsLadeBaseManager.Repository;
using YTSoft.CC.IGrains.XsLadeBaseManager;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.Grains.XsLadeBaseManager
{
    [Reentrant]
    public class XsLadeBaseGrain : MainGrain, IXsLadeBaseGrain
    {
        private readonly IXsLadeBaseRepository _xsLadeBaseRepository;
        private readonly IOrleansClient _orleansClient;

        public XsLadeBaseGrain(IXsLadeBaseRepository xsLadeBaseRepository, IOrleansClient orleansClient)
        {
            this._xsLadeBaseRepository = xsLadeBaseRepository;
            this._orleansClient = orleansClient;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<XsLadeBaseDO>> GetAllSync(XsLadeBaseDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _xsLadeBaseRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess(entities?.ToList());
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultPageLR<XsLadeBaseVO>> GetPageLRSync(SearchDTO<XsLadeBaseDTO> model)
        {
            var dto = model.Data;
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _xsLadeBaseRepository.CountAsync(expression, isMaster: false);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<XsLadeBaseVO>("无数据！");
            }

            var entities = await _xsLadeBaseRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);
            var scheduleTaskVOs = this.ObjectMapper.Map<List<XsLadeBaseVO>>(entities);
            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count, model.PageIndex, model.PageSize);
        }

        /// <summary>
        /// 根据企业主键和发货单编号，查发货单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult<XsLadeBaseVO>> GetByLadeIdSync(XsLadeBaseDTO model)
        {
            var expression = model.GetExpression2();
            var entitys = await _xsLadeBaseRepository.GetAllAsync(expression, isMaster: false);

            if (entitys.AsList().Count == 0)
            {
                return ApiResultUtil.IsFailed<XsLadeBaseVO>("当前企业该发货单编号不存在，请核对！");
            }
            else if (entitys.AsList().Count > 1)
            {
                return ApiResultUtil.IsFailed<XsLadeBaseVO>("当前企业该发货单编号存在多条，请核对！");
            }
            var xsLadeBaseVO = this.ObjectMapper.Map<XsLadeBaseVO>(entitys.FirstOrDefault());
            if (xsLadeBaseVO.XLB_Status == "2")
            {
                return ApiResultUtil.IsFailed<XsLadeBaseVO>("该发货单已冲红，请核对！");
            }

            //查客商名称
            if (!string.IsNullOrEmpty(xsLadeBaseVO.XLB_Client))
            {
                var xsCompyBaseResult = await _orleansClient.GetGrain<IXsCompyBaseStateGrain>(xsLadeBaseVO.XLB_Firm).GetAllSync(new XsCompyBaseDTO { Ids = new List<string> { xsLadeBaseVO.XLB_Client } });
                if (xsCompyBaseResult.Success && xsCompyBaseResult.Data.Count > 0)
                {
                    xsLadeBaseVO.XLB_ClientName = xsCompyBaseResult.Data[0].XOB_Name;
                }
            }
            //查企业名称
            if (!string.IsNullOrEmpty(xsLadeBaseVO.XLB_Firm))
            {
                var pbBasicFirmResult = await _orleansClient.GetGrain<IPbBasicFirmStateGrain>(GrainIdKey.UtcUIDGrainKey.ToString()).GetAllSync(new PbBasicFirmDTO { Ids = new List<string> { xsLadeBaseVO.XLB_Firm } });
                if (pbBasicFirmResult.Success && pbBasicFirmResult.Data.Count > 0)
                {
                    xsLadeBaseVO.XLB_FirmName = pbBasicFirmResult.Data[0].PBF_ShortName;
                }
            }
            //查物料名称
            if (!string.IsNullOrEmpty(xsLadeBaseVO.XLB_Cement))
            {
                var pbCodeMaterialResult = await _orleansClient.GetGrain<IPbCodeMaterialStateGrain>(xsLadeBaseVO.XLB_Firm).GetAllSync(new PbCodeMaterialDTO { Ids = new List<string> { xsLadeBaseVO.XLB_Cement } });
                if (pbCodeMaterialResult.Success && pbCodeMaterialResult.Data.Count > 0)
                {
                    xsLadeBaseVO.XLB_CementName = pbCodeMaterialResult.Data[0].PCM_Name;
                }
            }

            return await Task.FromResult(ApiResultUtil.IsSuccess(xsLadeBaseVO));
        }
    }
}
