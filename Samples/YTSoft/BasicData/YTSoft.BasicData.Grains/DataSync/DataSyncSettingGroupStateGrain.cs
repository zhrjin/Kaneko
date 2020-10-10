using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans.Concurrency;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataSync;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.Grains.DataSync
{
    [Reentrant]
    public class DataSyncSettingGroupStateGrain : StateGrain<string, DataSyncGroupState>, IDataSyncSettingGroupStateGrain
    {
        public DataSyncSettingGroupStateGrain()
        {
        }

        public Task<ApiResult<DataSyncGroupState>> GetAsync()
        {
            return Task.FromResult(ApiResultUtil.IsSuccess(this.State));
        }
    }
}
