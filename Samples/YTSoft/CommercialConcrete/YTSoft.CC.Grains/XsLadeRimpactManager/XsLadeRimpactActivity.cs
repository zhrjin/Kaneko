using Kaneko.Core.Contract;
using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Sagas;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeRimpactManager;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;

namespace YTSoft.CC.Grains.XsLadeRimpactManager
{
    /// <summary>
    /// 发货单作废
    /// </summary>
    [ServiceDescriptor(typeof(IActivity), ServiceLifetime.Transient)]
    public class XsLadeRimpactActivity : IActivity
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
                var data = context.SagaProperties.Get<SubmitDTO<XsLadeRimpactDTO>>("Data");

                var sourceGrain = context.GrainFactory.GetGrain<IXsLadeRimpactStateGrain>(grainId);
                await sourceGrain.SubmitAsync(context.SagaId, data);
            }
            else if ("ADD" == eventType)
            {
                var isDel = context.SagaProperties.Get<int>("IsDel");
                var userId = context.SagaProperties.Get<string>("UserId");
                var userName = context.SagaProperties.Get<string>("UserName");

                var sourceGrain = context.GrainFactory.GetGrain<IXsLadeRimpactStateGrain>(grainId);
                await sourceGrain.RevertAsync(context.SagaId, isDel, userId, userName);
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
            var data = context.SagaProperties.Get<SubmitDTO<XsLadeRimpactDTO>>("Data");

            var sourceGrain = context.GrainFactory.GetGrain<IXsLadeRimpactStateGrain>(grainId);
            await sourceGrain.RevertAsync(context.SagaId, data.Data.IsDel, data.UserId, data.UserName);
        }
    }
}
