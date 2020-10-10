using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeBaseManager;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class XsLadeBaseController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public XsLadeBaseController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        public Task<ApiResult<XsLadeBaseVO>> GetSync(string Id)
        {
            return factory.GetGrain<IXsLadeBaseStateGrain>(Id).GetAsync();
        }

        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<XsLadeBaseDTO> model)
        {
            //生成唯一ID
            string newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            model.Data.Id = newId;
            return await factory.GetGrain<IXsLadeBaseStateGrain>(newId).AddAsync(model);
        }

        [HttpPost("modity")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<XsLadeBaseDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsLadeBaseStateGrain>(model.Data.Id).UpdateAsync(model);
        }

        [HttpDelete]
        public Task<ApiResult> DeleteAsync(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsLadeBaseStateGrain>(Id).DeleteAsync();
        }

        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<XsLadeBaseVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<XsLadeBaseDTO>();
            return factory.GetGrain<IXsLadeBaseGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }

        /// <summary>
        /// 根据企业主键和发货单编号，查发货单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("query")]
        public Task<ApiResult<XsLadeBaseVO>> GetByLadeIdSync([FromQuery] XsLadeBaseDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.XLB_LadeId))
            {
                return Task.FromResult(ApiResultUtil.IsFailed<XsLadeBaseVO>("发货单编号不允许为空！"));
            }
            if (string.IsNullOrWhiteSpace(model.XLB_Firm))
            {
                return Task.FromResult(ApiResultUtil.IsFailed<XsLadeBaseVO>("所属企业不允许为空！"));
            }
            return factory.GetGrain<IXsLadeBaseGrain>(System.Guid.NewGuid().ToString()).GetByLadeIdSync(model);
        }
    }
}
