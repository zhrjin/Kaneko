using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Orleans.Runtime;
using Kaneko.Core.IdentityServer;
using System.Diagnostics;
using Kaneko.Core.Extensions;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 常用Grain
    /// </summary>
    public abstract class MainGrain : Grain, IIncomingGrainCallFilter, IOutgoingGrainCallFilter
    {
        protected ICurrentUser CurrentUser { get; private set; }

        /// <summary>
        /// Log
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// The real Type of the current Grain
        /// </summary>
        protected Type GrainType { get; }

        /// <summary>
        /// Primary key of actor
        /// Because there are multiple types, dynamic assignment in OnActivateAsync
        /// </summary>
        public string GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        public MainGrain()
        {
            this.GrainType = this.GetType();
        }

        public override Task OnActivateAsync()
        {
            GrainId = this.GetPrimaryKeyString();
            DependencyInjection();
            base.OnActivateAsync();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unified method of dependency injection
        /// </summary>
        /// <returns></returns>
        protected virtual Task DependencyInjection()
        {
            this.ObjectMapper = (IObjectMapper)this.ServiceProvider.GetService(typeof(IObjectMapper));
            this.Logger = (ILogger)this.ServiceProvider.GetService(typeof(ILogger<>).MakeGenericType(this.GrainType));
            return Task.CompletedTask;
        }

        /// <summary>
        /// 拦截器记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
                if (!string.IsNullOrEmpty(userData)) { CurrentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData); }

                await context.Invoke();

                stopWatch.Stop();

                long lElapsedMilliseconds = stopWatch.ElapsedMilliseconds;

                await Task.Run(() =>
                {
                    try
                    {
                        string sResult = "";
                        string sMessage = string.Format(
                              "NormalGrain-{0}.{1}({2}) returned value {3},耗时:{4}ms",
                              context.Grain.GetType().FullName,
                              context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                              (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
                              sResult,
                              lElapsedMilliseconds);
                        Logger.LogInfo(sMessage);

                        if (lElapsedMilliseconds > 3 * 1000)
                        {
                            //超3秒发出警告
                            //Logger.LogWarning(sMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        string sMessage = string.Format(
                             "{0}.{1}({2})",
                             context.Grain.GetType().FullName,
                             context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                             (context.Arguments == null ? "" : string.Join(", ", context.Arguments)));

                        Logger.LogError("记录日志报错：" + sMessage, ex);
                    }
                });
            }
            catch (Exception exception)
            {
                string sMessage = string.Format(
                       "{0}.{1}({2}) threw an exception：{3}",
                       context.Grain.GetType().FullName,
                       context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                       (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
                       exception);

                Logger.LogError("记录日志报错2", exception);

                throw;
            }
        }

        public Task Invoke(IOutgoingGrainCallContext context)
        {
            return context.Invoke();
        }
    }
}
