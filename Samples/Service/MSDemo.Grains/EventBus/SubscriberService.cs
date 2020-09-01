using DotNetCore.CAP;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MSDemo.Grains.EventBus
{
    public interface ISubscriberService
    {
        public void CheckReceivedMessage(DateTime datetime);
    }

    [ServiceDescriptor(typeof(ISubscriberService), ServiceLifetime.Transient)]
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        [CapSubscribe("kanoko.test1")]
        public void CheckReceivedMessage(DateTime datetime)
        {
        }
    }
}
