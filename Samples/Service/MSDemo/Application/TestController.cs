﻿using Kaneko.Core.ApiResult;
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
            return factory.GetGrain<ITestGrain>("111").GetResult(dto);
        }

        [HttpPost("CapBusTest")]
        public Task<ResultVO> CapBusTest()
        {
            return factory.GetGrain<ITestGrain>("111").CapBusTest();
        }
    }
}