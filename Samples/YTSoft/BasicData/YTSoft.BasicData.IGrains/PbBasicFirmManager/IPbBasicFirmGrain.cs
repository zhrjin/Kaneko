using Kaneko.Core.ApiResult;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;

namespace YTSoft.BasicData.IGrains.PbBasicFirmManager
{
    public interface IPbBasicFirmGrain : IGrainWithStringKey
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<PbBasicFirmDO>> GetAllSync(PbBasicFirmDTO model);
    }
}