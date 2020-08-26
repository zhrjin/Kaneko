﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Kaneko.Core.DependencyInjection
{
    internal class ImplementationTypeSelector : IImplementationTypeSelector, ISelector
    {
        public ImplementationTypeSelector(IAssemblySelector assemblySelector, IEnumerable<Type> types)
        {
            AssemblySelector = assemblySelector;
            Types = types;
            Selectors = new List<ISelector>();
        }

        private IEnumerable<Type> Types { get; }

        private List<ISelector> Selectors { get; }

        private IAssemblySelector AssemblySelector { get; }

        public IImplementationTypeSelector FromAssemblyOf<T>()
        {
            return AssemblySelector.FromAssemblyOf<T>();
        }

        public IImplementationTypeSelector FromAssembliesOf(params Type[] types)
        {
            return AssemblySelector.FromAssembliesOf(types);
        }

        public IImplementationTypeSelector FromAssembliesOf(IEnumerable<Type> types)
        {
            return AssemblySelector.FromAssembliesOf(types);
        }

        public IImplementationTypeSelector FromAssemblies(params Assembly[] assemblies)
        {
            return AssemblySelector.FromAssemblies(assemblies);
        }

        public IImplementationTypeSelector FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return AssemblySelector.FromAssemblies(assemblies);
        }


        public IImplementationTypeSelector FromRuntimeAssemblies(Func<AssemblyName, bool> predicate = null)
        {
            return AssemblySelector.FromRuntimeAssemblies(predicate);
        }

        public void AddFromAttributes()
        {
            AddFromAttributes(publicOnly: false);
        }

        public void AddFromAttributes(bool publicOnly)
        {
            Selectors.Add(new AttributeSelector(GetNonAbstractClasses(publicOnly)));
        }

        public void AddFromAttributes(Action<IImplementationTypeFilter> action)
        {
            AddFromAttributes(action, publicOnly: false);
        }

        public void AddFromAttributes(Action<IImplementationTypeFilter> action, bool publicOnly)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var filter = new ImplementationTypeFilter(GetNonAbstractClasses(publicOnly));

            action(filter);

            var selector = new AttributeSelector(filter.Types);

            Selectors.Add(selector);
        }

        public IServiceTypeSelector AddClasses()
        {
            return AddClasses(publicOnly: false);
        }

        public IServiceTypeSelector AddClasses(bool publicOnly)
        {
            return AddSelector(GetNonAbstractClasses(publicOnly));
        }

        public IServiceTypeSelector AddClasses(Action<IImplementationTypeFilter> action)
        {
            return AddClasses(action, publicOnly: false);
        }

        public IServiceTypeSelector AddClasses(Action<IImplementationTypeFilter> action, bool publicOnly)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var filter = new ImplementationTypeFilter(GetNonAbstractClasses(publicOnly));

            action(filter);

            return AddSelector(filter.Types);
        }

        void ISelector.Populate(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            foreach (var selector in Selectors)
            {
                selector.Populate(services);
            }
        }

        private IServiceTypeSelector AddSelector(IEnumerable<Type> types)
        {
            var selector = new ServiceTypeSelector(this, types);

            Selectors.Add(selector);

            return selector;
        }

        private IEnumerable<Type> GetNonAbstractClasses(bool publicOnly)
        {
            return Types.Where(t => t.IsNonAbstractClass(publicOnly));
        }

       
    }
}
