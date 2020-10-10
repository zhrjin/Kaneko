using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Orleans;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.DataDictionary.Model;

namespace YTSoft.BasicData.IGrains.DataDictionary
{
    public interface IDictDataTypeStateGrain : IGrainWithStringKey, IStateGrain<DictState>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<DictVO>> GetAsync(long id);

        /// <summary>
        /// 按照字典类别获取字典
        /// </summary>
        /// <returns></returns>
        Task<ApiResultList<DictVO>> GetListAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddAsync(SubmitDTO<DictDTO> model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> UpdateAsync(SubmitDTO<DictDTO> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteAsync(long id);
    }
}
