using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;
using Kaneko.Server.AutoMapper;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Kaneko.Server.Orleans.Grains
{
    /// <summary>
    /// 常用Grain
    /// </summary>
    [Reentrant]
    public abstract class MainGrain : Grain, IIncomingGrainCallFilter, IOutgoingGrainCallFilter
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

        public Task Invoke(IIncomingGrainCallContext context)
        {
         

            return context.Invoke();
        }

        public Task Invoke(IOutgoingGrainCallContext context)
        {
            return context.Invoke();
        }
    }
}
