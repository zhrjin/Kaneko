using Kaneko.Core.ApiResult;
using Kaneko.Hosts.Controller;
using Microsoft.AspNetCore.Mvc;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans;
using System.Threading.Tasks;

namespace MSDemo.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class TestController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public TestController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpPost("result")]
        public Task<ApiResult<TestVO>> GetResult([FromForm] TestDTO dto)
        {
            string ddd = System.Guid.NewGuid().ToString();
            return factory.GetGrain<ITestGrain>(ddd).GetResultTest1(dto);
        }

        [HttpPost("CapBusTest")]
        public Task<ApiResult> CapBusTest()
        {
            return factory.GetGrain<ITestGrain>("111").CapBusTest();
        }
    }
}