using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskBaseManager;
using YTSoft.CC.IGrains.XsTaskBaseManager.Domain;
using System;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class XsTaskBaseController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public XsTaskBaseController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public Task<ApiResult<XsTaskBaseVO>> GetSync(string id)
        {
            return factory.GetGrain<IXsTaskBaseStateGrain>(id).GetAsync();
        }

        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<XsTaskBaseDTO> model)
        {
            //生成唯一ID
            string newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            model.Data.Id = newId;
            return await factory.GetGrain<IXsTaskBaseStateGrain>(newId).AddAsync(model);
        }

        [HttpPost("modity")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<XsTaskBaseDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsTaskBaseStateGrain>(model.Data.Id).UpdateAsync(model);
        }

        [HttpDelete]
        public Task<ApiResult> DeleteAsync(string id)
        {
            return factory.GetGrain<IXsTaskBaseStateGrain>(id).DeleteAsync();
        }


        [HttpGet("refresh")]
        public Task<ApiResult> Refresh(string id)
        {
            return factory.GetGrain<IXsTaskBaseStateGrain>(id).ReinstantiateState();
        }

        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<XsTaskBaseVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<XsTaskBaseDTO>();
            return factory.GetGrain<IXsTaskBaseGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }

    }
}
