using Kaneko.Core.ApiResult;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;

namespace YTSoft.BasicData.IGrains.PbCodeMaterialManager
{
    public interface IPbCodeMaterialGrain : IGrainWithStringKey
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<PbCodeMaterialDO>> GetAllSync(PbCodeMaterialDTO model);
    }
}