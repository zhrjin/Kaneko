using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.Entity;
using YTSoft.CC.IGrains.Service;
using YTSoft.CC.IGrains.State;
using YTSoft.CC.IGrains.VO;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class ScheduleTaskController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public ScheduleTaskController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public Task<ApiResult<ScheduleTaskVO>> GetSync(long id)
        {
            return factory.GetGrain<IScheduleTaskStateGrain>(id).GetAsync();
        }

        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<ScheduleTaskDTO> model)
        {
            //生成唯一ID
            long newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
            model.Data.Id = newId;
            return await factory.GetGrain<IScheduleTaskStateGrain>(newId).AddAsync(model);
        }

        [HttpPost("update")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<ScheduleTaskDTO> model)
        {
            if (model.Data.Id <= 0)
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IScheduleTaskStateGrain>(model.Data.Id).UpdateAsync(model);
        }

        [HttpDelete]
        public Task<ApiResult> DeleteAsync(long id)
        {
            return factory.GetGrain<IScheduleTaskStateGrain>(id).DeleteAsync();
        }

        [HttpGet("list")]
        public Task<ApiResultPage<ScheduleTaskVO>> GetPageSync([FromQuery] SearchDTO<ScheduleTaskDTO> model)
        {
            return factory.GetGrain<IScheduleTaskGrain>(System.Guid.NewGuid().ToString()).GetPageSync(model);
        }

        [HttpGet("refresh")]
        public Task<ApiResult> Refresh(long id)
        {
            return factory.GetGrain<IScheduleTaskStateGrain>(id).ReinstantiateState();
        }

        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<ScheduleTaskVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<ScheduleTaskDTO>();
            return factory.GetGrain<IScheduleTaskGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }
    }
}