using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataDictionary.Model;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.IGrains.DataSync
{
    public interface IDataSyncSettingAllStateGrain : IGrainWithGuidKey, IStateGrain<DataSyncAllState>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SystemInfoAllDTO model);

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<DataSyncAllState>> GetAsync();

        /// <summary>
        /// 更新系统
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateSystemAsync(SystemInfoDO model);

        /// <summary>
        /// 新建表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddTableAsync(List<TableInfoDTO> model);

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateTableAsync(TableInfoDO model);

        /// <summary>
        /// 新建列
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddColumnAsync(List<ColumnInfoDTO> model);

        /// <summary>
        /// 更新列
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateColumnAsync(ColumnInfoDO model);

    }
}
