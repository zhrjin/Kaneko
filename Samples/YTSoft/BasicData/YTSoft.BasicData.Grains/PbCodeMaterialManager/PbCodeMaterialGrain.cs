using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager;
using YTSoft.BasicData.Grains.PbCodeMaterialManager.Repository;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;

namespace YTSoft.BasicData.Grains.PbCodeMaterialManager
{
    [Reentrant]
    public class MaterialGrain : MainGrain, IPbCodeMaterialGrain
    {
        private readonly IPbCodeMaterialRepository _pbCodeMaterialRepository;

        public MaterialGrain(IPbCodeMaterialRepository pbBasicFirmRepository)
        {
            this._pbCodeMaterialRepository = pbBasicFirmRepository;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultList<PbCodeMaterialDO>> GetAllSync(PbCodeMaterialDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _pbCodeMaterialRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess<PbCodeMaterialDO>(entities?.ToList());
        }
    }
}