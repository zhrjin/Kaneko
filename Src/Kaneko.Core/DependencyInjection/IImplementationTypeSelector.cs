using System;

namespace Kaneko.Core.DependencyInjection
{
    /// <summary>
    /// ����ѡ����
    /// </summary>
    public interface IImplementationTypeSelector : IAssemblySelector
    {
        /// <summary>
        /// ������й��л�ǳ����ൽ <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>��.
        /// </summary>
        IServiceTypeSelector AddClasses();

        /// <summary>
        /// ������й��л�ǳ����ൽ <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>��.
        /// </summary>
        /// <param name="publicOnly">Ҫ��ӵ������Ƿ��ǹ��е�</param>
        IServiceTypeSelector AddClasses(bool publicOnly);

        /// <summary>
        ///������й��л�ǳ����࣬���� <paramref name="action"/> ����ɸѡ��Ľ���� <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>��.
        /// </summary>
        /// <param name="action">���˺���</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="action"/> argument is <c>null</c>.</exception>
        IServiceTypeSelector AddClasses(Action<IImplementationTypeFilter> action);

        /// <summary>
        /// ������й��л�ǳ����࣬���� <paramref name="action"/>
        /// ����ɸѡ��Ľ���� <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
        /// </summary>
        /// <param name="action">���˺���</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="action"/> argument is <c>null</c>.</exception>
        /// <param name="publicOnly">Ҫ��ӵ������Ƿ��ǹ��е�</param>
        IServiceTypeSelector AddClasses(Action<IImplementationTypeFilter> action, bool publicOnly);
    }
}
