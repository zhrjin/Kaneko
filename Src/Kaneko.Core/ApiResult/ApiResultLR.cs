using Kaneko.Core.Contract;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kaneko.Core.ApiResult
{
    public class ApiResultLR<TVO> where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("rows")]
        public IList<TVO> Rows { set; get; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { set; get; }

        /// <summary>
        /// 当前页
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { set; get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonPropertyName("records")]
        public int Records { set; get; }
    }
}
