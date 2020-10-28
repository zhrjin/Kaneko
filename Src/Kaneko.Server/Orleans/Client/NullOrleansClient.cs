using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.Client
{
    public class NullOrleansClient : IOrleansClient
    {
        public void BindGrainReference(Assembly assembly, IAddressable grain)
        {
            throw new NotImplementedException();
        }

        public Task<TGrainObserverInterface> CreateObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            throw new NotImplementedException();
        }

        public Task DeleteObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            throw new NotImplementedException();
        }

        public IClusterClient GetClusterClient(Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            throw new NotImplementedException();
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            throw new NotImplementedException();
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            throw new NotImplementedException();
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            throw new NotImplementedException();
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            throw new NotImplementedException();
        }

        public IStreamProvider GetStreamProvider(Assembly assembly, string name)
        {
            throw new NotImplementedException();
        }
    }
}
