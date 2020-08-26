using Kaneko.Core.ApiResult;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.VO;
using Orleans;
using System.Threading.Tasks;

namespace MSDemo.IGrains.Service
{
    public interface ITestGrain : IGrainWithGuidKey
    {
        Task<DataResultVO<TestVO>> GetResult(TestDTO dto);
    }
}