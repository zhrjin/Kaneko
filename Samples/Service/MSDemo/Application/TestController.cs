using Kaneko.Core.ApiResult;
using Kaneko.Hosts.Controller;
using Microsoft.AspNetCore.Mvc;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace MSDemo.Application
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

        [HttpPost]
        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            var strings = this.HttpContext.Request.Headers["Authorization"].ToString();

            string ddd = System.Guid.NewGuid().ToString();
            dto.UserId = ddd;

            RequestContext.Set("ddd", ddd);

            //Task.Delay(5000).Wait();

            return factory.GetGrain<ITestGrain>(ddd).GetResult(dto);
        }

        [HttpPost("CapBusTest")]
        public Task<ResultVO> CapBusTest()
        {
            return factory.GetGrain<ITestGrain>("111").CapBusTest();
        }
    }
}