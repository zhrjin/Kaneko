using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;

namespace YTSoft.CC.IGrains.XsTaskDetailManager
{
    public interface IXsTaskDetailGrain : IGrainWithStringKey, IMainGrain
    {

    }
}
