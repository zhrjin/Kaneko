using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kaneko.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceDescriptorAttribute : Attribute
    {
        public ServiceDescriptorAttribute() : this(null) { }

        public ServiceDescriptorAttribute(Type serviceType) : this(serviceType, ServiceLifetime.Transient) { }

        public ServiceDescriptorAttribute(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        public ServiceDescriptorAttribute(string name, Type serviceType,
            ServiceLifetime lifetime = ServiceLifetime.Transient) : this(serviceType, lifetime)
        {
            this.Name = name;
        }

        public Type ServiceType { get; }

        public string Name { set; get; }

        public ServiceLifetime Lifetime { get; }
    }
}