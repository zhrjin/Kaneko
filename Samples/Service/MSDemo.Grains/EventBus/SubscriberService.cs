using DotNetCore.CAP;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Kaneko.Core.Extensions;

namespace MSDemo.Grains.EventBus
{
    public interface ISubscriberService
    {
        public void CheckReceivedMessage(DateTime datetime);
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


        [CapSubscribe("kanoko.test")]
        public void CheckReceivedMessage(DateTime datetime)
        {
            Logger.LogError("datetime=" + datetime.ToString(),new Exception("datetime=" + datetime.ToString()));
        }
    }
}
