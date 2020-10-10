using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskBaseManager.Domain;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.XsTaskBaseManager
{
    public interface IXsTaskBaseGrain : IGrainWithStringKey, IMainGrain
    {
        Task<ApiResultPageLR<XsTaskBaseVO>> GetPageLRSync(SearchDTO<XsTaskBaseDTO> model);
    }
}
