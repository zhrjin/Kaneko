using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.DependencyInjection;
using Kaneko.Core.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Threading.Tasks;
using TYSoft.Common.Model.ComConcrete;
using TYSoft.Common.Model.DataSync;
using TYSoft.Common.Model.EventBus;
using YTSoft.EventBus.Service.ComConcrete.Repository;
using YTSoft.EventBus.Service.Contracts;

namespace YTSoft.EventBus.Service.ComConcrete
{
    /// <summary>
    /// 任务单同步
    /// </summary>
    public interface ITaskListSyncService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="immutableModel"></param>
        /// <returns></returns>
        Task Execute(Immutable<EventData<TaskListModel>> immutableModel);
    }

    /// <summary>
    /// 任务单同步
    /// </summary>
    [ServiceDescriptor(typeof(ITaskListSyncService), ServiceLifetime.Transient)]
    public class TaskListSyncService : ITaskListSyncService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _redisCache;
        private readonly ITaskRepositoryFactory _repositoryFactory;

        public TaskListSyncService(ILogger<TaskListSyncService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IDistributedCache distributedCache, ITaskRepositoryFactory repositoryFactory)
        {
            _logger = logger;
            _config = configuration;
            _httpClientFactory = httpClientFactory;
            _redisCache = distributedCache;
            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="immutableModel"></param>
        /// <returns></returns>
        public async Task Execute(Immutable<EventData<TaskListModel>> immutableModel)
        {
            string groupCode = EventContract.ComConcrete.TaskListToZK;
            var client = _httpClientFactory.CreateClient(ConfigKeys.HttpClientName);
            var resultString = await client.GetStringAsync(_config[ConfigKeys.SYSDOMAIN_BASICDATA_DATASYNCSETTINGURL] + groupCode);
            var apiResult = JsonConvert.DeserializeObject<ApiResult<DataSyncModel>>(resultString);

            if (!apiResult.Success || apiResult.Data.Tables.Count == 0) { _logger.LogWarn($"{groupCode}未找到配置同步表！"); return; }

            var syncModel = immutableModel.Value.Data;
            var tables = apiResult.Data.Tables;

            if (tables.Count == 1)
            {
                await BeginSyncAsync(syncModel, tables[0]);
                return;
            }

            var cacheModels = new List<CacheModel>();
            string cacheKey = "TaskSync:" + groupCode + "-" + syncModel.BatchNo;
            string redisCacheString = await _redisCache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(redisCacheString)) { cacheModels = JsonConvert.DeserializeObject<List<CacheModel>>(redisCacheString); }

            var parallelLoop = Parallel.ForEach(tables, (tbl) =>
            {
                CacheModel cacheModel;
                if (cacheModels.Any(mbox => mbox.TableId == tbl.Id))
                {
                    cacheModel = cacheModels.First(mbox => mbox.TableId == tbl.Id);
                    if (cacheModel.IsSync) { return; }
                }
                else
                {
                    cacheModel = new CacheModel { TableId = tbl.Id };
                    cacheModels.Add(cacheModel);
                }
                bool bRet = BeginSyncAsync(syncModel, tbl).ConfigureAwait(false).GetAwaiter().GetResult();
                cacheModel.IsSync = bRet;
            });

            if (parallelLoop.IsCompleted)
            {
                if (cacheModels.Any(mbox => !mbox.IsSync))
                {
                    //有失败的
                    await _redisCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(cacheModels), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });

                    throw new Exception("同步失败!");//失败抛出异常，后续重试
                }
                else
                {
                    await _redisCache.RemoveAsync(cacheKey);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private async Task<bool> BeginSyncAsync(TaskListModel model, TableInfoModel table)
        {
            try
            {
                var repository = _repositoryFactory.Create(table.SystemInfo.DbConnection);
                var columns = table.Columns;
                var addFields = new List<string>();
                var atFields = new List<string>();
                foreach (var column in columns)
                {
                    addFields.Add($"{column.SelfColumn}");
                    atFields.Add($"@{column.ModelColumn}");
                }
                var sql = $"insert into {table.TableName}({string.Join(", ", addFields)}) values({string.Join(", ", atFields)});";
                var bRet = await repository.ExecuteSqlAsync(sql, model);
                return bRet;
            }
            catch (Exception ex)
            {
                _logger.LogError("BeginSyncAsync", ex);
                return false;
            }
        }

        /// <summary>
        /// 缓存项
        /// </summary>
        class CacheModel
        {
            public long TableId { set; get; }

            public bool IsSync { set; get; }
        }
    }
}
