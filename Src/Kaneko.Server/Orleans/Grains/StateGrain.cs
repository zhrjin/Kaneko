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
using Kaneko.Core.Exceptions;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 有状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    [StorageProvider(ProviderName = "RedisStore")]
    public abstract class StateGrain<TState> : Grain<TState>, IIncomingGrainCallFilter where TState : IState
    {
        /// <summary>
        /// 上下文用户信息
        /// </summary>
        protected ICurrentUser CurrentUser { get; private set; }

        /// <summary>
        /// Log
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// The real Type of the current Grain
        /// </summary>
        protected Type GrainType { get; }

        /// <summary>
        /// 事件转发器
        /// </summary>
        protected ICapPublisher Observer { get; set; }

        /// <summary>
        /// Primary key of actor
        /// Because there are multiple types, dynamic assignment in OnActivateAsync
        /// </summary>
        protected string GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        protected IObjectMapper ObjectMapper { get; set; }

        public StateGrain()
        {
            this.GrainType = this.GetType();
        }

        public override async Task OnActivateAsync()
        {
            GrainId = this.GetPrimaryKeyString();
            DependencyInjection();

            var onReadDbTask = OnReadFromDbAsync();
            if (!onReadDbTask.IsCompletedSuccessfully)
                await onReadDbTask;
            var result = onReadDbTask.Result;

            if (result != null)
            {
                State = result;
                await WriteStateAsync();
            }

            await base.OnActivateAsync();
        }

        /// <summary>
        /// Unified method of dependency injection
        /// </summary>
        /// <returns></returns>
        protected virtual void DependencyInjection()
        {
            this.ObjectMapper = (IObjectMapper)this.ServiceProvider.GetService(typeof(IObjectMapper));
            this.Logger = (ILogger)this.ServiceProvider.GetService(typeof(ILogger<>).MakeGenericType(this.GrainType));
            this.Observer = (ICapPublisher)this.ServiceProvider.GetService(typeof(ICapPublisher));
        }

        protected async Task Persist(ProcessAction action, TState state = default)
        {
            if (ProcessAction.Create == action || ProcessAction.Update == action)
            {
                this.State = state;
                await WriteStateAsync();
            }
            else if (ProcessAction.Delete == action)
            {
                await this.ClearStateAsync();
                this.DeactivateOnIdle();
            }
        }

        [Obsolete]
        private async Task ProcessChange(Func<ICapPublisher, Task<IStatable<TState>>> commandFunc, Action<Exception> errorFunc)
        {
            try
            {
                var statable = await commandFunc.Invoke(this.Observer);
                if (ProcessAction.Create == statable.GetAction() || ProcessAction.Update == statable.GetAction())
                {
                    this.State = statable.GetState();
                    await WriteStateAsync();
                }
                else if (ProcessAction.Delete == statable.GetAction())
                {
                    await this.ClearStateAsync();
                    this.DeactivateOnIdle();
                }
            }
            catch (KanekoException ex)
            {
                errorFunc(ex);
            }
            catch (Exception ex)
            {
                Logger.LogError("", ex);
                errorFunc(ex);
            }
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
                try
                {
                    string sMessage = string.Format(
                          "{0}.{1}({2}),耗时:{3}ms",
                          context.Grain.GetType().FullName,
                          context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                          (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
                          lElapsedMilliseconds);

                    Logger.LogInfo(sMessage);

                    if (lElapsedMilliseconds > 3 * 1000)
                    {
                        //超3秒发出警告
                        Logger.LogWarn("超时警告：" + sMessage);
                    }
                }
                catch { }
            }
            catch (Exception exception)
            {
                Logger.LogError("Grain执行异常：", exception);
                if (FuncExceptionHandler != null)
                {
                    await FuncExceptionHandler(exception);
                }
                throw exception;
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        protected virtual Func<Exception, Task> FuncExceptionHandler => (exception) =>
        {
            return Task.CompletedTask;
        };
    }
}
