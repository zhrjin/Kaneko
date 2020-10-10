using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain;

namespace YTSoft.CC.IGrains.XsProductMiddleDataManager
{
    public interface IXsProductMiddleDataGrain : IGrainWithStringKey, IMainGrain
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<XsProductMiddleDataDO>> GetAllSync(XsProductMiddleDataDTO model);

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPageLR<XsProductMiddleDataVO>> GetPageLRSync(SearchDTO<XsProductMiddleDataDTO> model);
    }
}
