using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using Orleans;
using System;
using YTSoft.CC.IGrains.TaskManager;
using YTSoft.CC.Grains.TaskManager.Repository;
using YTSoft.CC.IGrains.TaskManager.Domain;
using YTSoft.CC.IGrains.MaterialManager;
using YTSoft.CC.IGrains.MaterialManager.Domain;
using Kaneko.Core.Contract;
using YTSoft.BasicData.IGrains.DataDictionary;
using System.Net.Http;
using Kaneko.Core.Utils;
using Kaneko.Server.Orleans.Services;

namespace YTSoft.CC.Grains.TaskManager
{
    [Reentrant]
    public class ScheduleTaskGrain : MainGrain, IScheduleTaskGrain
    {
        private readonly IScheduleTaskRepository _scheduleRepository;
        private readonly IOrleansClient _clusterClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public ScheduleTaskGrain(IScheduleTaskRepository scheduleTaskRepository, IOrleansClient clusterClient, IHttpClientFactory httpClientFactory)
        {
            this._scheduleRepository = scheduleTaskRepository;
            this._clusterClient = clusterClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResultPage<ScheduleTaskVO>> GetPageSync(SearchDTO<ScheduleTaskDTO> model)
        {
            await Task.WhenAll(Enumerable.Range(0, 10).Select(x => NewLongID(x)));

            var dto = model.Data;
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _scheduleRepository.CountAsync(expression);
            if (count == 0) { return ApiResultUtil.IsFailedPage<ScheduleTaskVO>("无数据！"); }

            var entities = await _scheduleRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);

            List<ScheduleTaskVO> scheduleTaskVOs;

            if (entities.Any(m => !string.IsNullOrEmpty(m.ItemId)))
            {
                //获取物料数据
                var itemResult = await GrainFactory.GetGrain<IMaterialGrain>(this.GrainId).GetAllSync(new MaterialDTO
                {
                    Ids = entities.Where(m => !string.IsNullOrEmpty(m.ItemId)).Select(m => m.ItemId).Distinct().ToList()
                });

                var itemList = itemResult.Success ? itemResult.Data : null;
                if (itemList == null) { scheduleTaskVOs = this.ObjectMapper.Map<List<ScheduleTaskVO>>(entities); }
                else
                {
                    //左连接物料数据
                    scheduleTaskVOs = (from schedule in entities
                                       join item in itemList on schedule.ItemId equals item.Id into mapping
                                       from tmp in mapping.DefaultIfEmpty() //左连接需要加上DefaultIfEmpty
                                       select new
                                       {
                                           schedule,
                                           tmp
                                       }).AsEnumerable()
                                       .Select(x =>
                                       {
                                           var result = this.ObjectMapper.Map<ScheduleTaskVO>(x.schedule);
                                           result.ItemName = x.tmp?.ItemName;
                                           result.ItemNumber = x.tmp?.ItemNumber;
                                           return result;
                                       })
                                       .ToList();
                }
            }
            else
            {
                scheduleTaskVOs = this.ObjectMapper.Map<List<ScheduleTaskVO>>(entities);
            }

            //DictDataTypeStateGrain

            var ddd = await _clusterClient.GetGrain<IDictDataTypeStateGrain>("we").GetListAsync();

            var ddd2 = await this.GrainFactory.GetGrain<IScheduleTaskStateGrain>(1111).GetAsync();

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://127.0.0.1:6114");
            Dictionary<string, string> postForm = new Dictionary<string, string>
                {
                    { "Data.DataType", "TY" },
                    { "Data.DataCode", "A001"},
                    { "Data.DataValue", "V001" },
                    { "Data.DataDesc", "测试" },
                    { "Data.SortNo", "1" },
                    { "Data.Firm", "YT001" }
                };
            var task = await client.PostKanekoAsync("/BasicData/api/Dict/add", "", null, postForm);

            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count);
        }

        public async Task<ApiResultPageLR<ScheduleTaskVO>> GetPageLRSync(SearchDTO<ScheduleTaskDTO> model)
        {
            var dto = model.Data;
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _scheduleRepository.CountAsync(expression);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<ScheduleTaskVO>("无数据！");
            }

            var entities = await _scheduleRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);
            var scheduleTaskVOs = this.ObjectMapper.Map<List<ScheduleTaskVO>>(entities);
            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count, model.PageIndex, model.PageSize);
        }

        public async Task Test()
        {
            await Task.WhenAll(Enumerable.Range(0, 10).Select(x => NewLongID(x)));
        }
        private async Task NewLongID(int x)
        {
            var id2 = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            Console.WriteLine(id2);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        protected override Func<Exception, Task> FuncExceptionHandler => (exception) =>
        {
            return Task.CompletedTask;
        };
    }
}
