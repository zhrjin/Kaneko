using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsLadeRimpactManager;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;

namespace YTSoft.CC.Hosts.Controller
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class XsLadeRimpactController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public XsLadeRimpactController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 查看发货单作废
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<XsLadeRimpactVO>> GetSync(string Id)
        {
            return factory.GetGrain<IXsLadeRimpactStateGrain>(Id).GetAsync();
        }

        /// <summary>
        /// 新增发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<XsLadeRimpactDTO> model)
        {
            //生成唯一ID
            string newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            model.Data.Id = newId;
            return await factory.GetGrain<IXsLadeRimpactGrain>(newId).AddAsync(model);
        }

        /// <summary>
        /// 删除发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("del")]
        public Task<ApiResult> DelAsync([FromForm] SubmitDTO<XsLadeRimpactDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IXsLadeRimpactGrain>(model.Data.Id).DelAsync(model);
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<XsLadeRimpactVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<XsLadeRimpactDTO>();
            return factory.GetGrain<IXsLadeRimpactGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }
    }
}
