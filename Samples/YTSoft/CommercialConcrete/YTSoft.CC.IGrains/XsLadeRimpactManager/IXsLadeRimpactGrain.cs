using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager
{
    public interface IXsLadeRimpactGrain : IGrainWithStringKey, IMainGrain
    {
        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPageLR<XsLadeRimpactVO>> GetPageLRSync(SearchDTO<XsLadeRimpactDTO> model);

        /// <summary>
        /// 新增发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<XsLadeRimpactDTO> model);

        /// <summary>
        /// 删除发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> DelAsync(SubmitDTO<XsLadeRimpactDTO> model);
    }
}
