using Dapper;
using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskBaseManager;
using YTSoft.CC.Grains.XsTaskBaseManager.Repository;
using YTSoft.CC.IGrains.XsTaskBaseManager.Domain;

namespace YTSoft.CC.Grains.XsTaskBaseManager
{
    [Reentrant]
    public class XsTaskBaseGrain : MainGrain, IXsTaskBaseGrain
    {
        private readonly IXsTaskBaseRepository _xstaskbaseRepository;
        private readonly IClusterClient _clusterClient;

        public XsTaskBaseGrain(IXsTaskBaseRepository XsTaskBaseRepository, IClusterClient clusterClient)
        {
            this._xstaskbaseRepository = XsTaskBaseRepository;
            this._clusterClient = clusterClient;
        }

        public async Task<ApiResultPageLR<XsTaskBaseVO>> GetPageLRSync(SearchDTO<XsTaskBaseDTO> model)
        {
            var dto = model.Data;
            var expression = dto.GetPageExpression();
            var orders = dto.GetOrder();
            var count = await _xstaskbaseRepository.CountAsync(expression);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<XsTaskBaseVO>("无数据！");
            }

            var entities = await _xstaskbaseRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);
            var scheduleTaskVOs = this.ObjectMapper.Map<List<XsTaskBaseVO>>(entities);
            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count, model.PageIndex, model.PageSize);
        }
    }
}
