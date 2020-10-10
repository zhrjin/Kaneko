using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager
{
    public interface IXsLadeRimpactStateGrain : IGrainWithStringKey, IStateGrain<XsLadeRimpactState>
    {
        /// <summary>
        /// 查看发货单作废
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<XsLadeRimpactVO>> GetAsync();

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

        /// <summary>
        /// 提交发货单作废
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        Task SubmitAsync(Guid transactionId, SubmitDTO<XsLadeRimpactDTO> model);

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="isDel"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task RevertAsync(Guid transactionId, int isDel, string userId, string userName);
    }
}
