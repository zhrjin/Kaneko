using DotNetCore.CAP;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Kaneko.Core.Extensions;
using YTSoft.CC.IGrains.VO;

namespace YTSoft.CC.Grains.EventBus
{
    public interface ISubscriberService
    {
        public void CheckReceivedMessage(ScheduleTaskVO scheduleTaskVO);
    }

    [ServiceDescriptor(typeof(ISubscriberService), ServiceLifetime.Transient)]
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        /// <summary>
        /// Log
        /// </summary>
        public ILogger Logger { get; set; }

        public SubscriberService(ILogger<SubscriberService> logger)
        {
            Logger = logger;
        }


        [CapSubscribe(EventContract.TaskInterface)]
        public void CheckReceivedMessage(ScheduleTaskVO scheduleTaskVO)
        {

        }
    }
}
