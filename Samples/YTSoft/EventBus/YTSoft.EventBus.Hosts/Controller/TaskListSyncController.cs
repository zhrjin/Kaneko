using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using TYSoft.Common.Model.EventBus;
using TYSoft.Common.Model.ComConcrete;
using YTSoft.EventBus.Service.ComConcrete;
using Orleans.Concurrency;
using System.Threading.Tasks;
using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;

namespace YTSoft.EventBus.Hosts.Controller
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class TaskListSyncController : ControllerBase
    {
        private readonly ITaskListSyncService _taskListSyncService;
        private readonly ICapPublisher _capPublisher;

        public TaskListSyncController(ITaskListSyncService taskListSyncService, ICapPublisher capPublisher)
        {
            _taskListSyncService = taskListSyncService;
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="model"></param>
        [HttpPost("pub")]
        public async Task<ApiResult> PublishMessage(TaskListModel model)
        {
            await _capPublisher.PublishAsync(EventContract.ComConcrete.TaskListToZK, model);
            return ApiResultUtil.IsSuccess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        [CapSubscribe(EventContract.ComConcrete.TaskListToZK)]
        public Task CheckReceivedMessage(EventData<TaskListModel> model)
        {
            return _taskListSyncService.Execute(new Immutable<EventData<TaskListModel>>(model));
        }
    }
}
