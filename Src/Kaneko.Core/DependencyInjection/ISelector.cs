using Microsoft.Extensions.DependencyInjection;

namespace Kaneko.Core.DependencyInjection
{
    /// <summary>
    /// Ñ¡ÔñÆ÷
    /// </summary>
    internal interface ISelector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        void Populate(IServiceCollection services);
    }
}