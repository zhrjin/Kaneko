using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsProductMiddleDataManager;
using YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class XsProductMiddleDataController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public XsProductMiddleDataController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public Task<ApiResult<XsProductMiddleDataVO>> GetSync(string Id)
        {
            return factory.GetGrain<IXsProductMiddleDataStateGrain>(Id).GetAsync();
        }

        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<XsProductMiddleDataDTO> model)
        {
            //生成唯一ID
            string newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            model.Data.Id = newId;
            return await factory.GetGrain<IXsProductMiddleDataStateGrain>(newId).AddAsync(model);
        }

        [HttpPost("modity")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<XsProductMiddleDataDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsProductMiddleDataStateGrain>(model.Data.Id).UpdateAsync(model);
        }

        [HttpDelete]
        public Task<ApiResult> DeleteAsync(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsProductMiddleDataStateGrain>(Id).DeleteAsync();
        }

        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<XsProductMiddleDataVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<XsProductMiddleDataDTO>();
            return factory.GetGrain<IXsProductMiddleDataGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }
    }
}
