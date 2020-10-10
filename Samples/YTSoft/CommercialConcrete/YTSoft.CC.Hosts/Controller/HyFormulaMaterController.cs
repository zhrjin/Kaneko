using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.HyFormulaMaterManager;
using YTSoft.CC.IGrains.HyFormulaMaterManager.Domain;

namespace YTSoft.CC.Hosts.Controller
{
    /// <summary>
    /// 物料名称配置
    /// </summary>
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class HyFormulaMaterController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public HyFormulaMaterController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResult<HyFormulaMaterVO>> GetSync(string Id)
        {
            return factory.GetGrain<IHyFormulaMaterStateGrain>(Id).GetAsync();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ApiResult> AddAsync([FromForm] SubmitDTO<HyFormulaMaterDTO> model)
        {
            string newId = await factory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();
            model.Data.Id = newId;
            return await factory.GetGrain<IHyFormulaMaterStateGrain>(newId).AddAsync(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("modity")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<HyFormulaMaterDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IHyFormulaMaterStateGrain>(model.Data.Id).UpdateAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("del")]
        public Task<ApiResult> DelAsync([FromForm] SubmitDTO<HyFormulaMaterDTO> model)
        {
            if (string.IsNullOrWhiteSpace(model.Data.Id))
            {
                return Task.FromResult(ApiResultUtil.IsFailed("Id不能为空！"));
            }
            return factory.GetGrain<IHyFormulaMaterStateGrain>(model.Data.Id).DelAsync(model);
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("list-lr")]
        public Task<ApiResultPageLR<HyFormulaMaterVO>> GetPageLRSync([FromQuery] SearchDTO search)
        {
            var model = search.DeserializeObject<HyFormulaMaterDTO>();
            return factory.GetGrain<IHyFormulaMaterGrain>(System.Guid.NewGuid().ToString()).GetPageLRSync(model);
        }
    }
}
