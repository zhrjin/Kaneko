using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;

namespace YTSoft.BasicData.IGrains.PbCodeMaterialManager
{
    public interface IPbCodeMaterialStateGrain : IGrainWithStringKey, IStateGrain<PbCodeMaterialState>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<PbCodeMaterialDO>> GetAllSync(PbCodeMaterialDTO model);
    }
}