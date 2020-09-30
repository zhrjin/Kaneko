using SkyApm.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyWalkingDemo2
{
    public class CustomSamplingInterceptor : ISamplingInterceptor
    {
        public int Priority { get; } = 0;

        public bool Invoke(SamplingContext samplingContext, Sampler next)
        {
            return next(samplingContext);
        }
    }
}
