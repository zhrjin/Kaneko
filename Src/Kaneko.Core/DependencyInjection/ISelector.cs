using Microsoft.Extensions.DependencyInjection;

namespace Kaneko.Core.DependencyInjection
{
    /// <summary>
    /// ѡ����
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