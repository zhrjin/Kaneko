using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using System;
using YTSoft.CC.IGrains.MaterialManager;
using YTSoft.CC.Grains.MaterialManager.Repository;
using YTSoft.CC.IGrains.MaterialManager.Domain;

namespace YTSoft.CC.Grains.MaterialManager
{
    [Reentrant]
    public class MaterialGrain : MainGrain, IMaterialGrain
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialGrain(IMaterialRepository materialRepository)
        {
            this._materialRepository = materialRepository;
        }

        public async Task<ApiResultList<MaterialDO>> GetAllSync(MaterialDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var entities = await _materialRepository.GetAllAsync(expression, isMaster: false, orderByFields: orders);
            return ApiResultUtil.IsSuccess(entities?.ToList());
        }

        public async Task<ApiResult<MaterialDO>> GetSync(MaterialDTO model)
        {
            var expression = model.GetExpression();
            var entities = await _materialRepository.GetAsync(expression);
            return ApiResultUtil.IsSuccess(entities);
        }
    }
}