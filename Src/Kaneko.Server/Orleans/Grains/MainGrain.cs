using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Orleans.Runtime;
using Kaneko.Core.IdentityServer;
using System.Diagnostics;
using Kaneko.Core.Extensions;
using DotNetCore.CAP;
using Kaneko.Server.Orleans.Services;
using Kaneko.Core.DependencyInjection;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using StackExchange.Profiling;
using System.Collections.Generic;
using Kaneko.Server.SkyAPM.Orleans.Diagnostic;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 常用Grain
    /// </summary>
    public abstract class MainGrain : Grain, IIncomingGrainCallFilter
    {
        /// <summary>
        /// 上下文用户信息
        /// </summary>
        protected ICurrentUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
                    if (!string.IsNullOrEmpty(userData))
                    {
                        _currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData);
                    }
                }
                return _currentUser;
            }
        }

        /// <summary>
        /// 上下文用户信息
        /// </summary>
        private ICurrentUser _currentUser;

        /// <summary>
        /// Log
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// 事件转发器
        /// </summary>
        protected ICapPublisher Observer { get; private set; }

        /// <summary>
        /// The real Type of the current Grain
        /// </summary>
        protected Type GrainType { get; }

        /// <summary>
        /// Primary key of actor
        /// Because there are multiple types, dynamic assignment in OnActivateAsync
        /// </summary>
        protected string GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; private set; }

        private static readonly DiagnosticListener _diagnosticListener =
       new DiagnosticListener(KanekoDiagnosticListenerNames.DiagnosticListenerName);

        public MainGrain()
        {
            this.GrainType = this.GetType();
        }

        public override Task OnActivateAsync()
        {
            GrainId = this.GetPrimaryKeyString();
            DependencyInjection();
            base.OnActivateAsync();
            OnActivateNextAsync();
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
        /// 拦截器记录日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public async Task Invoke(IIncomingGrainCallContext context)
        //{
        //    string OperId = System.Guid.NewGuid().ToString();
        //    var timer = Stopwatch.StartNew();
        //    try
        //    {
        //        var profiler = MiniProfiler.StartNew("StartNew");
        //        using (profiler.Step("BaseGrain"))
        //        {
        //            string userData = RequestContext.Get(IdentityServerConsts.ClaimTypes.UserData) as string;
        //            if (!string.IsNullOrEmpty(userData)) { CurrentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userData); }

        //            if (_diagnosticListener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeBefore))
        //            {
        //                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //                var excuteData = new KanekoExcuteData
        //                {
        //                    OperatioName = context.Grain.GetType().Name + "." + context.InterfaceMethod?.Name,
        //                    GrainId = OperId,
        //                    GrainType = context.Grain.GetType().FullName,
        //                    GrainMethod = context.InterfaceMethod?.Name
        //                };
        //                _diagnosticListener.Write(KanekoDiagnosticListenerNames.OrleansInvokeBefore, excuteData);
        //            }

        //            await context.Invoke();

        //            if (_diagnosticListener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeAfter))
        //            {
        //                var excuteData = new KanekoExcuteData
        //                {
        //                    GrainId = OperId,
        //                    GrainType = context.Grain.GetType().FullName,
        //                    GrainMethod = context.InterfaceMethod?.Name,
        //                    OperatioName = context.Grain.GetType().Name + "." + context.InterfaceMethod?.Name,
        //                };
        //                _diagnosticListener.Write(KanekoDiagnosticListenerNames.OrleansInvokeAfter, excuteData);
        //            }

        //            timer.Stop();
        //            double lElapsedMilliseconds = timer.Elapsed.TotalMilliseconds;
        //            try
        //            {
        //                string sMessage = string.Format(
        //                      "{0}.{1}({2}),耗时:{3}ms",
        //                      context.Grain.GetType().FullName,
        //                      context.InterfaceMethod == null ? "" : context.InterfaceMethod.Name,
        //                      (context.Arguments == null ? "" : string.Join(", ", context.Arguments)),
        //                      lElapsedMilliseconds.ToString("0.00"));

        //                Logger.LogInfo(sMessage);

        //                if (lElapsedMilliseconds > 3 * 1000)
        //                {
        //                    //超3秒发出警告
        //                    Logger.LogWarn("超时警告：" + sMessage);
        //                }
        //            }
        //            catch { }
        //        }

        //        SqlProfilerLog(profiler);
        //    }
        //    catch (Exception exception)
        //    {
        //        timer.Stop();
        //        double lElapsedMilliseconds = timer.Elapsed.TotalMilliseconds;
        //        Logger.LogError($"Grain执行异常,耗时:{lElapsedMilliseconds:0.00}ms", exception);
        //        if (FuncExceptionHandler != null)
        //        {
        //            await FuncExceptionHandler(exception);
        //        }

        //        if (_diagnosticListener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeError))
        //        {
        //            var excuteData = new KanekoExcuteData
        //            {
        //                GrainId = OperId,
        //                GrainType = context.Grain.GetType().FullName,
        //                GrainMethod = context.InterfaceMethod?.Name,
        //                Exception = exception,
        //                OperatioName = context.Grain.GetType().Name + "." + context.InterfaceMethod?.Name,
        //            };
        //            _diagnosticListener.Write(KanekoDiagnosticListenerNames.OrleansInvokeError, excuteData);
        //        }
        //        throw exception;
        //    }
        //}

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
    }
}
