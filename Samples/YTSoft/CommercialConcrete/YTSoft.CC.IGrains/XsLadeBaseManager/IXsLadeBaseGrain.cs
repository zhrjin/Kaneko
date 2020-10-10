using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.IGrains.XsLadeBaseManager
{
    public interface IXsLadeBaseGrain : IGrainWithStringKey, IMainGrain
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<XsLadeBaseDO>> GetAllSync(XsLadeBaseDTO model);

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultPageLR<XsLadeBaseVO>> GetPageLRSync(SearchDTO<XsLadeBaseDTO> model);

        /// <summary>
        /// 根据企业主键和发货单编号，查发货单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<XsLadeBaseVO>> GetByLadeIdSync(XsLadeBaseDTO model);
    }
}
