using Kaneko.Core.ApiResult;
using Kaneko.Hosts.Controller;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataDictionary.Model;
using YTSoft.BasicData.IGrains.DataSync;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.Hosts.Controller
{
    /// <summary>
    /// 数据同步配置
    /// </summary>
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class DataSyncSettingController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public DataSyncSettingController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add-system")]
        public Task<ApiResult> AddAsync(SystemInfoAllDTO model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).AddAsync(model);
        }

        [HttpPost("add-column")]
        public Task<ApiResult> AddColumnAsync(List<ColumnInfoDTO> model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).AddColumnAsync(model);
        }

        [HttpPost("add-table")]
        public Task<ApiResult> AddTableAsync(List<TableInfoDTO> model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).AddTableAsync(model);
        }

        [HttpGet("list")]
        public Task<ApiResult<DataSyncAllState>> GetAsync()
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).GetAsync();
        }

        [HttpGet("refresh-all")]
        public Task<ApiResult> ReinstantiateState()
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).ReinstantiateState();
        }

        [HttpPost("modity-column")]
        public Task<ApiResult> UpdateColumnAsync(ColumnInfoDO model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).UpdateColumnAsync(model);
        }

        [HttpPost("modity-system")]
        public Task<ApiResult> UpdateSystemAsync(SystemInfoDO model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).UpdateSystemAsync(model);
        }

        [HttpPost("modity-table")]
        public Task<ApiResult> UpdateTableAsync(TableInfoDO model)
        {
            return factory.GetGrain<IDataSyncSettingAllStateGrain>(System.Guid.Empty).UpdateTableAsync(model);
        }

        [HttpGet("group")]
        public Task<ApiResult<DataSyncGroupState>> GetGroupAsync(string groupCode)
        {
            return factory.GetGrain<IDataSyncSettingGroupStateGrain>(groupCode).GetAsync();
        }
    }
}
