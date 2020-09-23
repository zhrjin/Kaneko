using Kaneko.Core.Contract;
using System.Text.Json.Serialization;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 分页列表数据结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultPageLR<TVO> : ApiResult where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public ApiResultLR<TVO> Data { set; get; }
    }
}
