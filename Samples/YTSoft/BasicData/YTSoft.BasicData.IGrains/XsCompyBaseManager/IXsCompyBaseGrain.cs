using Kaneko.Core.ApiResult;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;

namespace YTSoft.BasicData.IGrains.XsCompyBaseManager
{
    public interface IXsCompyBaseGrain : IGrainWithStringKey
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<XsCompyBaseDO>> GetAllSync(XsCompyBaseDTO model);
    }
}