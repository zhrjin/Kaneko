using Kaneko.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace YTSoft.EventBus.Service.ComConcrete.Repository
{
    public interface ITaskRepositoryFactory
    {
        TaskRepository Create(string connectionString);
    }

    [ServiceDescriptor(typeof(ITaskRepositoryFactory), ServiceLifetime.Transient)]
    public class TaskRepositoryFactory : ITaskRepositoryFactory
    {
        public TaskRepository Create(string connectionString)
        {
            var configSource = new Dictionary<string, string>
            {
                ["ConnectionStrings:master"] = connectionString
            };

            var builder = new ConfigurationBuilder().AddInMemoryCollection(configSource).Build();
            var repository = new TaskRepository(builder);
            return repository;
        }
    }
}
