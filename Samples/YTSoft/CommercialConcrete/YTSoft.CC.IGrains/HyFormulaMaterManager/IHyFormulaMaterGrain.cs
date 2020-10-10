using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.HyFormulaMaterManager.Domain;

namespace YTSoft.CC.IGrains.HyFormulaMaterManager
{
    /// <summary>
    /// 物料名称配置
    /// </summary>
    public interface IHyFormulaMaterGrain : IGrainWithStringKey, IMainGrain
    {
        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPageLR<HyFormulaMaterVO>> GetPageLRSync(SearchDTO<HyFormulaMaterDTO> model);
    }
}
