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
using YTSoft.CC.Grains.XsProductMiddleDataManager.Repository;
using YTSoft.CC.IGrains.XsProductMiddleDataManager;
using YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain;

namespace YTSoft.CC.Grains.XsProductMiddleDataManager
{
    [Reentrant]
    public class XsProductMiddleDataGrain : MainGrain, IXsProductMiddleDataGrain
    {
        private readonly IXsProductMiddleDataRepository _xsLadeBaseRepository;
        private readonly IOrleansClient _orleansClient;

        public XsProductMiddleDataGrain(IXsProductMiddleDataRepository xsLadeBaseRepository, IOrleansClient orleansClient)
        {
            this._xsLadeBaseRepository = xsLadeBaseRepository;
            this._orleansClient = orleansClient;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<XsProductMiddleDataDO>> GetAllSync(XsProductMiddleDataDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _xsLadeBaseRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess<XsProductMiddleDataDO>(entities?.ToList());
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultPageLR<XsProductMiddleDataVO>> GetPageLRSync(SearchDTO<XsProductMiddleDataDTO> model)
        {
            var dto = model.Data;
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _xsLadeBaseRepository.CountAsync(expression, isMaster: false);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<XsProductMiddleDataVO>("无数据！");
            }

            var entities = await _xsLadeBaseRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);
            var scheduleTaskVOs = this.ObjectMapper.Map<List<XsProductMiddleDataVO>>(entities);
            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count, model.PageIndex, model.PageSize);
        }
    }
}
