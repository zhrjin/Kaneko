using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using MSDemo.IGrains.Entity;
using MSDemo.IGrains.Service;
using MSDemo.IGrains.State;
using MSDemo.IGrains.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSDemo.Grains.Service
{
    public class StateTestGrain : StateGrain<TestState>, IStateTestGrain
    {
        public Task<DataResultVO<TestVO>> GetResult(TestDTO dto)
        {
            var ddd = this.State;
            this.State.UID = System.DateTime.Now.ToString();
            this.WriteStateAsync();
            var result= ApiResultUtil.IsSuccess(new TestVO());
            return Task.FromResult(result);
        }
    }
}
