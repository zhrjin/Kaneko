using IdGen;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.Services
{
    [Reentrant]
    public class UtcUIDGrain : Grain, IUtcUID
    {
        private static readonly DateTime _utcEpoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private IdGenerator _unitIdGen;

        public override Task OnActivateAsync()
        {
            int grainId = (int)this.GetPrimaryKeyLong();
            _unitIdGen = new IdGenerator(grainId, new IdGeneratorOptions(timeSource: new DefaultTimeSource(_utcEpoch)));
            return base.OnActivateAsync();
        }

        public Task<string> NewID()
        {
            long id = _unitIdGen.CreateId();
            return Task.FromResult(id.ToString());
        }

        public Task<long> NewLongID()
        {


            return Task.FromResult(_unitIdGen.CreateId());
        }

        public Task<IEnumerable<long>> TakeNewID(int count)
        {
            return Task.FromResult(_unitIdGen.Take(count));
        }
    }
}
