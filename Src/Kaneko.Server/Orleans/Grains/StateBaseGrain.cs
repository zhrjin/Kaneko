using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Kaneko.Core.Extensions;
using Microsoft.Extensions.Logging;
using Orleans;
using Kaneko.Core.IdentityServer;
using Orleans.Runtime;
using DotNetCore.CAP;
using Kaneko.Core.Contract;
using System.Diagnostics;
using Kaneko.Core.ApiResult;
using Kaneko.Core.Attributes;
using Kaneko.Core.DependencyInjection;
using Kaneko.Server.Orleans.Services;
using StackExchange.Profiling;
using System.Collections.Generic;
using Kaneko.Server.SkyAPM.Orleans.Diagnostic;
using Kaneko.Core.Utils;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 有状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class StateBaseGrain<PrimaryKey, TState> : Grain<TState>, IIncomingGrainCallFilter where TState : class, IState
    {
        private static readonly DiagnosticListener _diagnosticListener =
          new DiagnosticListener(KanekoDiagnosticListenerNames.DiagnosticListenerName);

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

        //private long TimestampOnActivate { get; set; }

        public StateBaseGrain()
        {
            this.GrainType = this.GetType();
        }

        public override Task OnActivateAsync()
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

            //string sw8 = RequestContext.Get(IdentityServerConsts.ClaimTypes.SkyWalking) as string;
            //TimestampOnActivate = _diagnosticListener.OrleansOnActivate(this.GrainType, GrainId.ToString(), this.RuntimeIdentity, sw8);

            DependencyInjection();
            base.OnActivateAsync();
            return Task.CompletedTask;
        }

        public virtual Task OnActivateNextAsync()
        {
            return Task.CompletedTask;
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
            string sw8 = RequestContext.Get(IdentityServerConsts.ClaimTypes.SkyWalking) as string;
            string OperId = this.GrainId.ToString();
            var tracingTimestamp = _diagnosticListener.OrleansInvokeBefore(context.Grain.GetType(), context.InterfaceMethod, OperId, this.RuntimeIdentity, sw8);
            try
            {
                string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
                if (!string.IsNullOrEmpty(userData)) { CurrentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData); }

                await context.Invoke();

                _diagnosticListener.OrleansInvokeAfter(tracingTimestamp);
            }
            catch (Exception exception)
            {
                Logger.LogError($"Grain执行异常", exception);
                if (FuncExceptionHandler != null)
                {
                    await FuncExceptionHandler(exception);
                }

                _diagnosticListener.OrleansInvokeError(tracingTimestamp, exception);
                throw exception;
            }
        }

        /// <summary>
        /// sql跟踪
        /// </summary>
        /// <param name="profiler"></param>
        private void SqlProfilerLog(MiniProfiler profiler)
        {
            try
            {
                if (profiler?.Root != null)
                {
                    var root = profiler.Root;
                    if (root.HasChildren)
                    {
                        root.Children.ForEach(chil =>
                        {
                            if (chil.CustomTimings?.Count > 0)
                            {
                                foreach (var customTiming in chil.CustomTimings)
                                {
                                    var errSql = new List<string>();
                                    var allSql = new List<string>();
                                    var warnSql = new List<string>();
                                    int i = 1;
                                    customTiming.Value?.ForEach(value =>
                                    {
                                        var msg = $@"【{customTiming.Key}{i++}】{value.CommandString} Execute time :{value.DurationMilliseconds} ms,Start offset :{value.StartMilliseconds} ms,Errored :{value.Errored}";
                                        if (value.Errored)
                                            errSql.Add(msg);
                                        if (value.DurationMilliseconds >= 3 * 1000)
                                            warnSql.Add(msg);
                                        allSql.Add(msg);
                                    });

                                    if (errSql.Count > 0)
                                        Logger.LogError("异常sql:\r\n" + string.Join("\r\n", errSql), new Exception());

                                    if (warnSql.Count > 0)
                                        Logger.LogWarn("超时sql:\r\n" + string.Join("\r\n", warnSql));

                                    Logger.LogInfo("sql:\r\n" + string.Join("\r\n", allSql));
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Profiler", ex);
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

        /// <summary>
        /// 获取状态值
        /// </summary>
        /// <returns></returns>
        public virtual Task<TState> GetState()
        {
            //var temp = this.State.ToObjectCopy();
            var temp = this.State;
            return Task.FromResult(temp);
        }

    }
}
