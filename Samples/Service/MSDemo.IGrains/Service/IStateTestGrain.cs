using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.VO;
using System.Threading.Tasks;

namespace MSDemo.IGrains.Service
{
    public interface IStateTestGrain : IStateGrain
    {
        Task<DataResultVO<TestVO>> GetResult(TestDTO dto);
    }
}