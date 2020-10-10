using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskDetailManager;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;
using System;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class XsTaskDetailController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public XsTaskDetailController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public Task<ApiResult<XsTaskDetailVO>> GetSync(string id)
        {
            return factory.GetGrain<IXsTaskDetailStateGrain>(id).GetAsync();
        }

        //[HttpPost("add")]
        //public async Task<ApiResult> AddAsync([FromForm] XsTaskDetailDTO model)
        //{
        //    //生成唯一ID
        //    string newId = Guid.NewGuid().ToString();
        //    model.Id = newId;
        //    return await factory.GetGrain<IXsTaskDetailStateGrain>(newId).AddAsync(model);
        //}

        //[HttpPost("modity")]
        //public Task<ApiResult> UpdateAsync([FromForm] XsTaskDetailDTO model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.Id))
        //    {
        //        return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
        //    }
        //    return factory.GetGrain<IXsTaskDetailStateGrain>(model.Id).UpdateAsync(model);
        //}

        [HttpDelete]
        public Task<ApiResult> DeleteAsync(string id)
        {
            return factory.GetGrain<IXsTaskDetailStateGrain>(id).DeleteAsync();
        }


        [HttpGet("refresh")]
        public Task<ApiResult> Refresh(string id)
        {
            return factory.GetGrain<IXsTaskDetailStateGrain>(id).ReinstantiateState();
        }

    }
}
