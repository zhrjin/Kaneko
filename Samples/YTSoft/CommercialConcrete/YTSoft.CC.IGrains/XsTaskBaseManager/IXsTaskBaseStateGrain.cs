using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using Kaneko.Core.Contract;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskBaseManager.Domain;

namespace YTSoft.CC.IGrains.XsTaskBaseManager
{
    public interface IXsTaskBaseStateGrain : IGrainWithStringKey, IStateGrain<XsTaskBaseState>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<XsTaskBaseVO>> GetAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<XsTaskBaseDTO> model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<XsTaskBaseDTO> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync();
    }
}
