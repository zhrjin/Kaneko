using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.CC.IGrains.XsReceReceivableManager;
using YTSoft.CC.Grains.XsReceReceivableManager.Repository;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.Grains.XsReceReceivableManager
{
    [Reentrant]
    public class XsReceReceivableGrain : MainGrain, IXsReceReceivableGrain
    {
        private readonly IXsReceReceivableRepository _xsReceReceivableRepository;

        public XsReceReceivableGrain(IXsReceReceivableRepository pbBasicFirmRepository)
        {
            this._xsReceReceivableRepository = pbBasicFirmRepository;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<XsReceReceivableDO>> GetAllSync(XsReceReceivableDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _xsReceReceivableRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess<XsReceReceivableDO>(entities?.ToList());
        }
    }
}