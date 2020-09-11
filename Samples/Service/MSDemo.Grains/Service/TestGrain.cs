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

        public TestGrain(ITestRepository testRepository/*, ICapPublisher capBus, IDistributedCache cache*/)
        {
            //this._testRepository = testRepository;
            //this._capBus = capBus;
            //this._cache = cache;
        }

        public async Task<ApiResult<TestVO>> GetResultTest1(TestDTO dto)
        {
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();

            //var count = await _testRepository.CountAsync(expression);

            //var updateResult1 = await _testRepository.SetAsync(() => new { user_id = "eeeee" }, oo => oo.UserId == "4444");

            TestDO demoDO = new TestDO { UserId = dto?.UserId, UserName = this.CurrentUser?.UserName };

            //var updateResult3 = await _testRepository.SetAsync(demoDO);

            TestVO demoVO = this.ObjectMapper.Map<TestVO>(demoDO);
            var result = ApiResultUtil.IsSuccess(demoVO);
            return await Task.FromResult(result);
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

        /// <summary>
        /// 异常处理
        /// </summary>
        protected override Func<Exception, Task> FuncExceptionHandler => (exception) =>
        {
            return Task.CompletedTask;
        };

    }
}