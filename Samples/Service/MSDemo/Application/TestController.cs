using Kaneko.Core.ApiResult;
using Microsoft.AspNetCore.Mvc;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans;
using System;
using System.Threading.Tasks;

namespace MSDemo.Application
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IGrainFactory factory;

        public TestController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpPost]
        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            return factory.GetGrain<ITestGrain>(Guid.Empty).GetResult(dto);
        }
    }
}