using Kaneko.Server.Orleans.Services;
using Orleans.ApplicationParts;
using System.Reflection;

namespace Orleans
{
    public static class ApplicationPartManagerExtensions
    {
        public static void AddKanekoParts(this IApplicationPartManager manager)
        {
            manager.AddApplicationPart(typeof(UtcUIDGrain).Assembly).WithReferences();
        }
    }
}
