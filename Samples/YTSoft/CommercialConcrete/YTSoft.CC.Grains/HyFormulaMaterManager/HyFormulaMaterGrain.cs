using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans.Concurrency;
using System.Collections.Generic;
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
    public class HyFormulaMaterGrain : MainGrain, IHyFormulaMaterGrain
    {
        private readonly IHyFormulaMaterRepository _hyFormulaMaterRepository;

        public HyFormulaMaterGrain(IHyFormulaMaterRepository hyFormulaMaterRepository)
        {
            this._hyFormulaMaterRepository = hyFormulaMaterRepository;
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultPageLR<HyFormulaMaterVO>> GetPageLRSync(SearchDTO<HyFormulaMaterDTO> model)
        {
            var dto = model.Data;

            //根据查询条件获取分页列表数据
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _hyFormulaMaterRepository.CountAsync(expression);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<HyFormulaMaterVO>("无数据！");
            }
            var entities = await _hyFormulaMaterRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);

            var hyFormulaMaterVOs = this.ObjectMapper.Map<List<HyFormulaMaterVO>>(entities);

            return ApiResultUtil.IsSuccess(hyFormulaMaterVOs, count, model.PageIndex, model.PageSize);
        }
    }
}
