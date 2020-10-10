using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.MaterialManager.Domain;

namespace YTSoft.CC.IGrains.MaterialManager
{
    /// <summary>
    /// 物料
    /// </summary>
    public interface IMaterialGrain : IGrainWithStringKey
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<MaterialDO>> GetAllSync(MaterialDTO model);

        /// <summary>
        /// 获取单笔数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<MaterialDO>> GetSync(MaterialDTO model);
    }
}