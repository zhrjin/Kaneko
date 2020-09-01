using Kaneko.Core.ApiResult;
using Kaneko.Hosts.Controller;
using Microsoft.AspNetCore.Mvc;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans;
using System.Threading.Tasks;

namespace MSDemo.Application
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class StateTestController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public StateTestController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpPost]
        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            var ddd= factory.GetGrain<IStateTestGrain>("111").GetResult(dto);

         var dd=    ddd.Result;

            return ddd;
        } 
    }
}