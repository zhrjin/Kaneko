using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.IGrains.DataSync
{
    public interface IDataSyncSettingGroupStateGrain : IGrainWithStringKey, IStateGrain<DataSyncGroupState>
    {
        /// <summary>
        /// 所以分组下的所有需对接的配置信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<DataSyncGroupState>> GetAsync();
    }
}
