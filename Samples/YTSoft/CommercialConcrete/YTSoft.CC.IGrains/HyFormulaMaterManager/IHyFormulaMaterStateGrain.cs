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
    public interface IHyFormulaMaterStateGrain : IGrainWithStringKey, IStateGrain<HyFormulaMaterState>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<HyFormulaMaterVO>> GetAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<HyFormulaMaterDTO> model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<HyFormulaMaterDTO> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> DelAsync(SubmitDTO<HyFormulaMaterDTO> model);
    }
}
