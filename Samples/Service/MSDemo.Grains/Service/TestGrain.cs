using Kaneko.Core.ApiResult;
using Kaneko.Core.Orleans.Grains;
using MSDemo.Grains.Repository;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.VO;
using System;
using System.Threading.Tasks;

namespace MSDemo.Grains.Service
{
    public class TestGrain : NormalGrain<Guid>, ITestGrain
    {
        private readonly ITestRepository testRepository;

        public TestGrain(ITestRepository testRepository)
        {
            this.testRepository = testRepository;
        }

        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            TestDO demoDO = new TestDO { UserId = "234" };
            TestVO demoVO = this.ObjectMapper.Map<TestVO>(demoDO);
            var result = ApiResultUtil.IsSuccess(demoVO);
            return Task.FromResult(result);
        }
    }
}