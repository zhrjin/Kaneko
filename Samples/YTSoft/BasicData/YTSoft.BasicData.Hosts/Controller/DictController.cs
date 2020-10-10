using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Hosts.Controller;
using Kaneko.Server.Orleans.Services;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataDictionary;
using YTSoft.BasicData.IGrains.DataDictionary.Model;
using YTSoft.BasicData.IGrains.XsCompyBaseManager;

namespace YTSoft.BasicData.Hosts.Controller
{
    /// <summary>
    /// 数据字典
    /// </summary>
    [ApiVersion("1")]
    [Route("api/[controller]")]
    public class DictController : KaneKoController
    {
        private readonly IGrainFactory factory;

        public DictController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public Task<ApiResult> AddAsync([FromForm] SubmitDTO<DictDTO> model)
        {
            if (string.IsNullOrEmpty(model.Data.DataType)) { return Task.FromResult(ApiResultUtil.IsFailed("字典类型不能为空！")); }
            return factory.GetGrain<IDictDataTypeStateGrain>(model.Data.DataType).AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("remove")]
        public Task<ApiResult> DeleteAsync(string dataType, long id)
        {
            if (string.IsNullOrEmpty(dataType)) { return Task.FromResult(ApiResultUtil.IsFailed("字典类型不能为空！")); }
            return factory.GetGrain<IDictDataTypeStateGrain>(dataType).DeleteAsync(id);
        }

        /// <summary>
        /// 按照字典类型获取单个字典数据
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("query")]
        public Task<ApiResult<DictVO>> GetAsync(string dataType, long id)
        {
            if (string.IsNullOrEmpty(dataType)) { return Task.FromResult(ApiResultUtil.IsFailed<DictVO>("字典类型不能为空！")); }
            return factory.GetGrain<IDictDataTypeStateGrain>(dataType).GetAsync(id);
        }

        [HttpGet("all")]
        public Task<ApiResultList<DictVO>> GetListAsync(string dataType)
        {
            if (string.IsNullOrEmpty(dataType)) { return Task.FromResult(ApiResultUtil.IsFailedList<DictVO>("字典类型不能为空！")); }
            return factory.GetGrain<IDictDataTypeStateGrain>(dataType).GetListAsync();
        }

        [HttpPost("modity")]
        public Task<ApiResult> UpdateAsync([FromForm] SubmitDTO<DictDTO> model)
        {
            if (string.IsNullOrEmpty(model.Data.DataType)) { return Task.FromResult(ApiResultUtil.IsFailed("字典类型不能为空！")); }
            return factory.GetGrain<IDictDataTypeStateGrain>(model.Data.DataType).UpdateAsync(model);
        }

        [HttpGet("refresh")]
        public async Task<ApiResult> Refresh(string dataType)
        {
            return await factory.GetGrain<IDictDataTypeStateGrain>(dataType).ReinstantiateState();
        }
    }
}