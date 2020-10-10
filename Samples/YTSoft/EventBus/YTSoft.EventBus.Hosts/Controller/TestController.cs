using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using TYSoft.Common.Model.EventBus;
using TYSoft.Common.Model.ComConcrete;
using System.Threading.Tasks;
using TYSoft.Common.Model.Test;
using Kaneko.Core.Contract;

namespace YTSoft.EventBus.Hosts.Controller
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        public TestController()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [CapSubscribe(EventContract.Tester.Test)]
        public async Task CheckReceivedMessage(EventData<TestModel> model)
        {
            await Task.Delay(120);
        }
    }
}
