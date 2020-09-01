using Orleans;
using Orleans.Runtime;
using Orleans.Serialization;
using System.Threading.Tasks;

namespace Kaneko.Core.Orleans.Filter
{
    public class IncomingGrainCallFilter : IIncomingGrainCallFilter
    {
        private readonly SerializationManager serializationManager;
        private readonly Silo silo;
        private readonly IGrainFactory grainFactory;

        public IncomingGrainCallFilter(SerializationManager serializationManager, Silo silo, IGrainFactory grainFactory)
        {
            this.serializationManager = serializationManager;
            this.silo = silo;
            this.grainFactory = grainFactory;
        }

        public Task Invoke(IIncomingGrainCallContext context)
        {
            return context.Invoke();
        }
    }
}
