using DotNetCore.CAP;
using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using MSDemo.Grains.Repository;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace MSDemo.Grains.Service
{
    public class TestGrain : MainGrain, ITestGrain
    {
        private readonly ITestRepository testRepository;
        private readonly ICapPublisher capBus;

        public TestGrain(ITestRepository testRepository, ICapPublisher capPublisher)
        {
            this.testRepository = testRepository;
            capBus = capPublisher;
        }

        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            var dd = RequestContext.Get("ddd").ToString();

            TestDO demoDO = new TestDO { UserId = dd, UserName = dto.UserId };
            TestVO demoVO = this.ObjectMapper.Map<TestVO>(demoDO);
            var result = ApiResultUtil.IsSuccess(demoVO);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        public Task<ResultVO> CapBusTest()
        {
            //发送消息给客户端，第一个参值数"kjframe.test"为消息队列的topic
            capBus.PublishAsync("kanoko.test", DateTime.Now);
            var result = ApiResultUtil.IsSuccess();
            return Task.FromResult(result);
        }

    }
}