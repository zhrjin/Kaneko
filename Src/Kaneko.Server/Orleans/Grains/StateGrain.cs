using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Kaneko.Core.Extensions;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;
using Kaneko.Core.IdentityServer;
using Orleans.Runtime;

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

        public StateGrain()
        {
            this.GrainType = this.GetType();
        }

        public override Task OnActivateAsync()
        {
            GrainId = this.GetPrimaryKeyString();
            DependencyInjection();

            var onReadDbTask = OnReadFromDbAsync();
            var result = onReadDbTask.Result;
            if (result != null)
            {
                State = result;
                WriteStateAsync();
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
            return Task.CompletedTask;
        }

        protected virtual ValueTask<TState> OnReadFromDbAsync() => new ValueTask<TState>();

        /// <summary>
        /// 拦截器记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
                if (!string.IsNullOrEmpty(userData)) { CurrentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData); }

                await context.Invoke();

                await Task.Run(() =>
                {
                    try
                    {
                        //var ShortLogs = context.InterfaceMethod == null ? null : context.InterfaceMethod.GetCustomAttribute<ShortLogsAttribute>();
                        string sResult = "";// ShortLogs == null ? JsonConvert.SerializeObject(context.Result) : "结果不记录";

                        string sMessage = string.Format(
                              "{0}.{1}({2}) returned value {3}",
                              context.Grain.GetType().FullName,
                              context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                              (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
                              sResult);
                        Logger.LogInfo(sMessage);
                    }
                    catch (Exception ex)
                    {
                        string sMessage = string.Format(
                             "{0}.{1}({2})",
                             context.Grain.GetType().FullName,
                             context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                             (context.Arguments == null ? "" : string.Join(", ", context.Arguments)));

                        Logger.LogError("记录日志报错", ex);
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
