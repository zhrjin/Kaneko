using DotNetCore.CAP;
using Kaneko.Core.Contract;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;
using TYSoft.Common.Model.ComConcrete;
using TYSoft.Common.Model.EventBus;
using YTSoft.CC.IGrains.TaskManager;
using YTSoft.CC.IGrains.TaskManager.Domain;

namespace YTSoft.CC.Grains.TaskManager
{
    [ServiceDescriptor(typeof(IEventHandler), ServiceLifetime.Transient)]
    public class ScheduleTaskHandler : IEventHandler, ICapSubscribe
    {
        /// <summary>
        /// Log
        /// </summary>
        public ILogger Logger { get; set; }

        public IClusterClient _client;

        public ScheduleTaskHandler(ILogger<ScheduleTaskHandler> logger, IClusterClient client)
        {
            Logger = logger;
            _client = client;
        }

        /// <summary>
        /// 接收到的数据
        /// </summary>
        /// <param name="eventData"></param>
        [CapSubscribe(EventContract.ComConcrete.WithTransactionTest)]
        public async Task CheckReceivedMessage(EventData<TaskListModel> eventData)
        {
            //TODO
            //var ddd = await _client.GetGrain<IScheduleTaskGrain>("11").GetPageSync(new SearchDTO<ScheduleTaskDTO> { Data = new ScheduleTaskDTO() });

        }

    }
}
