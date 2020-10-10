using Kaneko.Core.Contract;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Sagas;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsReceReceivableManager;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.Grains.XsLadeRimpactManager
{
    [ServiceDescriptor(typeof(IActivity), ServiceLifetime.Transient)]
    public class XsReceReceivableActivity : IActivity
    {
        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IActivityContext context)
        {
            var grainId = context.SagaProperties.Get<string>("GrainId");
            var eventType = context.SagaProperties.Get<string>("EventType");

            if ("UPDATE" == eventType)
            {
                var isDel = context.SagaProperties.Get<int>("IsDel");
                var userId = context.SagaProperties.Get<string>("UserId");
                var userName = context.SagaProperties.Get<string>("UserName");

                var sourceGrain = context.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(grainId);
                await sourceGrain.RevertAsync(context.SagaId, isDel, userId, userName);
            }
            else if ("ADD" == eventType)
            {
                var data = context.SagaProperties.Get<SubmitDTO<XsReceReceivableDTO>>("Data");
                var sourceGrain = context.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(grainId);
                await sourceGrain.SubmitAsync(context.SagaId, data);
            }
        }

        /// <summary>
        /// 事务回滚事务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Compensate(IActivityContext context)
        {
            var grainId = context.SagaProperties.Get<string>("GrainId");
            var eventType = context.SagaProperties.Get<string>("EventType");
            int isDel = "ADD" == eventType ? 1 : 0;
            var userId = context.SagaProperties.Get<string>("UserId");
            var userName = context.SagaProperties.Get<string>("UserName");

            var sourceGrain = context.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(grainId);
            await sourceGrain.RevertAsync(context.SagaId, isDel, userId, userName);
        }
    }
}
