using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.IGrains.XsReceReceivableManager
{
    public interface IXsReceReceivableGrain : IGrainWithStringKey, IMainGrain
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<XsReceReceivableDO>> GetAllSync(XsReceReceivableDTO model);
    }
}