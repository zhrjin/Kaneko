using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;

namespace YTSoft.BasicData.IGrains.PbBasicFirmManager
{
    public interface IPbBasicFirmStateGrain : IGrainWithStringKey, IStateGrain<PbBasicFirmState>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<PbBasicFirmDO>> GetAllSync(PbBasicFirmDTO model);
    }
}