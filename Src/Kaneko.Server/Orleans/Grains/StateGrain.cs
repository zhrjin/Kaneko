using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Kaneko.Core.Extensions;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Kaneko.Core.IdentityServer;
using Orleans.Runtime;
using DotNetCore.CAP;
using Kaneko.Core.Contract;
using System.Diagnostics;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 有状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [StorageProvider(ProviderName = "RedisStore")]
    public abstract class StateGrain<TState> : Grain<TState>, IIncomingGrainCallFilter where TState : new()
    {
        protected ICurrentUser CurrentUser { get; private set; }

        /// <summary>
        /// Log
        /// </summary>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// The real Type of the current Grain
        /// </summary>
        protected Type GrainType { get; }

        /// <summary>
        /// 事件转发器
        /// </summary>
        protected ICapPublisher CapPublisher { get; private set; }

        /// <summary>
        /// Primary key of actor
        /// Because there are multiple types, dynamic assignment in OnActivateAsync
        /// </summary>
        public string GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        public StateGrain()
        {
            this.GrainType = this.GetType();
        }

        public override Task OnActivateAsync()
        {
            GrainId = this.GetPrimaryKeyString();
            DependencyInjection();

            var onReadDbTask = OnReadFromDbAsync();
            if (onReadDbTask != null)
            {
                var result = onReadDbTask.Result;
                if (result != null)
                {
                    State = result;
                    WriteStateAsync();
                }
            }

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
            this.CapPublisher = (ICapPublisher)this.ServiceProvider.GetService(typeof(ICapPublisher));

            return Task.CompletedTask;
        }


        protected virtual Task RaiseEvent<TEventData>(TState currentState, IEvent<TEventData> eventData)
        {
            this.State = currentState;
            this.WriteStateAsync();
            CapPublisher.PublishAsync(eventData.EventCode, eventData.Data);
            return Task.CompletedTask;
        }

        protected virtual Task<TState> OnReadFromDbAsync() => null;

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
    }
}
