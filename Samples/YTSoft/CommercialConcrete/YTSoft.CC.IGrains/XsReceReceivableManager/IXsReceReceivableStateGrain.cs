using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.IGrains.XsReceReceivableManager
{
    public interface IXsReceReceivableStateGrain : IGrainWithStringKey, IStateGrain<XsReceReceivableState>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<XsReceReceivableDTO> model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<XsReceReceivableDTO> model);

        Task SubmitAsync(Guid transactionId, SubmitDTO<XsReceReceivableDTO> model);

        Task RevertAsync(Guid transactionId, int isDel, string userId, string userName);
    }
}
