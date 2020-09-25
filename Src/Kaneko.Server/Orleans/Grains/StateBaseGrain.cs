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
using Kaneko.Core.ApiResult;
using Kaneko.Core.Attributes;
using Kaneko.Core.DependencyInjection;
using Kaneko.Server.Orleans.Services;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 有状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class StateBaseGrain<PrimaryKey, TState> : Grain<TState>, IIncomingGrainCallFilter where TState : IState
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
        protected PrimaryKey GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        protected IObjectMapper ObjectMapper { get; set; }

        public StateBaseGrain()
        {
            this.GrainType = this.GetType();
        }

        public override async Task OnActivateAsync()
        {
            var type = typeof(PrimaryKey);
            if (type == typeof(long) && this.GetPrimaryKeyLong() is PrimaryKey longKey)
                GrainId = longKey;
            else if (type == typeof(string) && this.GetPrimaryKeyString() is PrimaryKey stringKey)
                GrainId = stringKey;
            else if (type == typeof(Guid) && this.GetPrimaryKey() is PrimaryKey guidKey)
                GrainId = guidKey;
            else
                throw new ArgumentOutOfRangeException(typeof(PrimaryKey).FullName);

            DependencyInjection();
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

        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual async Task Persist(ProcessAction action, TState state = default)
        {
            if (ProcessAction.Create == action || ProcessAction.Update == action)
            {
                this.State = state;
                this.State.GrainDataState = GrainDataState.Loaded;
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
        /// 重置状态数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ApiResult> ReinstantiateState(TState state = default)
        {
            try
            {
                TState result = (TState)state;
                if (result == null)
                {
                    var onReadDbTask = OnReadFromDbAsync();
                    if (!onReadDbTask.IsCompletedSuccessfully)
                        await onReadDbTask;
                    result = onReadDbTask.Result;
                }

                if (result != null)
                {
                    State = result;
                    await WriteStateAsync();
                }
                else
                {
                    await ClearStateAsync();
                }

                return ApiResultUtil.IsSuccess();
            }
            catch (Exception ex)
            {
                Logger.LogError("刷新状态", ex);
                return ApiResultUtil.IsFailed(ex.Message);
            }
        }

        /// <summary>
        /// 拦截器记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
                if (!string.IsNullOrEmpty(userData)) { CurrentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData); }

                await context.Invoke();

                timer.Stop();
                double lElapsedMilliseconds = timer.Elapsed.TotalMilliseconds;
                try
                {
                    string sMessage = string.Format(
                          "{0}.{1}({2}),耗时:{3}ms",
                          context.Grain.GetType().FullName,
                          context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
                          (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
                          lElapsedMilliseconds.ToString("0.00"));

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
                timer.Stop();
                double lElapsedMilliseconds = timer.Elapsed.TotalMilliseconds;
                Logger.LogError($"Grain执行异常,耗时:{lElapsedMilliseconds:0.00}ms", exception);
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

        /// <summary>
        /// 消息发布
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        protected async Task PublishAsync<TEventData>(TEventData eventData)
        {
            var attribute = typeof(TEventData).GetKanekoAttribute<EventNameAttribute>();
            if (attribute == null) { throw new Exception($"{typeof(TEventData).FullName}缺少特性EventNameAttribute"); }

            var eventName = attribute.Name;

            EventData<TEventData> @event = new EventData<TEventData>
            {
                GrainId = this.GrainId.ToString(),
                GrainType = this.GrainType.Name,
                TransactionId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID(),
                Data = eventData
            };
            await Observer.PublishAsync(eventName, @event);
        }
    }
}
