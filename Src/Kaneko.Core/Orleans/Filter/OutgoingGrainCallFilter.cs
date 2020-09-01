using Orleans;
using Orleans.Runtime;
using Orleans.Serialization;
using System.Threading.Tasks;

namespace Kaneko.Core.Orleans.Filter
{
    public class OutgoingGrainCallFilter : IOutgoingGrainCallFilter
    {
        public Task Invoke(IOutgoingGrainCallContext context)
        {
            return context.Invoke();
        }
    }
}
