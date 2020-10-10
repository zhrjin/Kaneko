using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Sagas;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeBaseManager;

namespace YTSoft.CC.Grains.XsLadeRimpactManager
{
    [ServiceDescriptor(typeof(IActivity), ServiceLifetime.Transient)]
    public class XsLadeBaseActivity : IActivity
    {
        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IActivityContext context)
        {
            var grainId = context.SagaProperties.Get<string>("GrainId");
            var status = context.SagaProperties.Get<string>("XLB_Status");
            var userId = context.SagaProperties.Get<string>("UserId");
            var userName = context.SagaProperties.Get<string>("UserName");

            var sourceGrain = context.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(grainId);
            await sourceGrain.UpdateStatusAsync(context.SagaId, status, userId, userName);
        }

        /// <summary>
        /// 事务回滚事务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Compensate(IActivityContext context)
        {
            var grainId = context.SagaProperties.Get<string>("GrainId");
            var status = context.SagaProperties.Get<string>("XLB_Status");
            var userId = context.SagaProperties.Get<string>("UserId");
            var userName = context.SagaProperties.Get<string>("UserName");

            var sourceGrain = context.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(grainId);
            await sourceGrain.RevertStatusAsync(context.SagaId, status, userId, userName);
        }
    }
}
