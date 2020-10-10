using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.XsCompyBaseManager;
using YTSoft.BasicData.Grains.XsCompyBaseManager.Repository;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;

namespace YTSoft.BasicData.Grains.XsCompyBaseManager
{
    [Reentrant]
    public class XsCompyBaseGrain : MainGrain, IXsCompyBaseGrain
    {
        private readonly IXsCompyBaseRepository _pbBasicFirmRepository;

        public XsCompyBaseGrain(IXsCompyBaseRepository pbBasicFirmRepository)
        {
            this._pbBasicFirmRepository = pbBasicFirmRepository;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<XsCompyBaseDO>> GetAllSync(XsCompyBaseDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _pbBasicFirmRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess(entities?.ToList());
        }
    }
}