using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.VO;
using System.Threading.Tasks;

namespace MSDemo.IGrains.Service
{
    public interface ITestGrain : IMainGrain
    {
        Task<ApiResult<TestVO>> GetResultTest1(TestDTO dto);
        Task<ApiResultPage<TestVO>> GetResultTest2(TestDTO dto);

        Task<ApiResult> CapBusTest();
    }
}