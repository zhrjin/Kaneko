using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;

namespace YTSoft.BasicData.IGrains.XsCompyBaseManager
{
    public interface IXsCompyBaseStateGrain : IGrainWithStringKey, IStateGrain<XsCompyBaseState>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<XsCompyBaseDO>> GetAllSync(XsCompyBaseDTO model);
    }
}