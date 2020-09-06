using DotNetCore.CAP;
using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using Microsoft.Extensions.Caching.Distributed;
using MSDemo.Grains.Repository;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSDemo.Grains.Service
{
    public class TestGrain : MainGrain, ITestGrain
    {
        private readonly ITestRepository _testRepository;
        private readonly ICapPublisher _capBus;
        private readonly IDistributedCache _cache;

        public TestGrain(ITestRepository testRepository, ICapPublisher capBus, IDistributedCache cache)
        {
            this._testRepository = testRepository;
            this._capBus = capBus;
            this._cache = cache;
        }

        public Task<ApiResult<TestVO>> GetResultTest1(TestDTO dto)
        {
            TestDO demoDO = new TestDO { UserId = this.CurrentUser.UserId, UserName = this.CurrentUser.UserName };
            TestVO demoVO = this.ObjectMapper.Map<TestVO>(demoDO);
            var result = ApiResultUtil.IsSuccess(demoVO);
            return Task.FromResult(result);
        }

        public Task<ApiResultPage<TestVO>> GetResultTest2(TestDTO dto)
        {
            var dd = RequestContext.Get("ddd").ToString();
            List<TestDO> demoDO = new List<TestDO>() { new TestDO { UserId = dd, UserName = dto.UserId } };
            List<TestVO> demoVO = this.ObjectMapper.Map<List<TestVO>>(demoDO);
            var result = ApiResultUtil.IsSuccess(demoVO, 2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        public Task<ApiResult> CapBusTest()
        {
            //发送消息给客户端，第一个参值数"kjframe.test"为消息队列的topic
            _capBus.PublishAsync("kanoko.test", DateTime.Now);
            var result = ApiResultUtil.IsSuccess();
            return Task.FromResult(result);
        }

    }
}