using Kaneko.Core.IdentityServer;
using Kaneko.Server.SkyAPM.Orleans.Diagnostic;
using Orleans.Runtime;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public class SagaBaseGrain<TState> : Grain<TState>, IIncomingGrainCallFilter
    {
        private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener(KanekoDiagnosticListenerNames.DiagnosticListenerName);

        /// <summary>
        /// 拦截器记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            string sw8 = RequestContext.Get(IdentityServerConsts.ClaimTypes.SkyWalking) as string;
            string OperId = this.GetPrimaryKey().ToString();
            var tracingTimestamp = _diagnosticListener.OrleansInvokeBefore(context.Grain.GetType(), context.InterfaceMethod, OperId, this.RuntimeIdentity, sw8);
            try
            {
                await context.Invoke();
                _diagnosticListener.OrleansInvokeAfter(tracingTimestamp, context.Grain.GetType(), context.InterfaceMethod, OperId, this.RuntimeIdentity);
            }
            catch (Exception exception)
            {
                _diagnosticListener.OrleansInvokeError(tracingTimestamp, context.Grain.GetType(), context.InterfaceMethod, OperId, this.RuntimeIdentity, exception);
                throw exception;
            }
        }
    }
}
