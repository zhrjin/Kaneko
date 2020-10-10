using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.PbBasicFirmManager;
using YTSoft.BasicData.Grains.PbBasicFirmManager.Repository;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;

namespace YTSoft.BasicData.Grains.PbBasicFirmManager
{
    [Reentrant]
    public class PbBasicFirmGrain : MainGrain, IPbBasicFirmGrain
    {
        private readonly IPbBasicFirmRepository _pbBasicFirmRepository;

        public PbBasicFirmGrain(IPbBasicFirmRepository pbBasicFirmRepository)
        {
            this._pbBasicFirmRepository = pbBasicFirmRepository;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<PbBasicFirmDO>> GetAllSync(PbBasicFirmDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _pbBasicFirmRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess(entities?.ToList());
        }
    }
}