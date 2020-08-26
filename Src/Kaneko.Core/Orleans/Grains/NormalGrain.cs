using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using Kaneko.Core.Extensions;
using Kaneko.Core.AutoMapper;
using Orleans.Concurrency;

namespace Kaneko.Core.Orleans.Grains
{
    /// <summary>
    /// 常用Grain
    /// </summary>
    [Reentrant]
    public abstract class NormalGrain<PrimaryKey> : Grain, IIncomingGrainCallFilter
    {
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
        public PrimaryKey GrainId { get; private set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        public NormalGrain()
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

            var dITask = DependencyInjection();
            if (!dITask.IsCompletedSuccessfully)
            {
                await dITask;
            }

            await base.OnActivateAsync();
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

                Logger.LogError("记录日志报错2：" + sMessage, exception);

                throw;
            }
        }
    }
}
