using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.IGrains.XsLadeBaseManager
{
    public interface IXsLadeBaseStateGrain : IGrainWithStringKey, IStateGrain<XsLadeBaseState>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<XsLadeBaseVO>> GetAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<XsLadeBaseDTO> model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<XsLadeBaseDTO> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync();

        Task UpdateStatusAsync(Guid transactionId, string XLBStatus, string userId, string userName);

        Task RevertStatusAsync(Guid transactionId, string XLBStatus, string userId, string userName);
    }
}
