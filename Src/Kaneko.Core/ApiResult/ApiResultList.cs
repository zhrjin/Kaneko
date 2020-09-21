using Kaneko.Core.Contract;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 列表数据结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultList<TVO> : ApiResult where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
     
        [JsonPropertyName("data")]
        public IList<TVO> Data { set; get; }
    }
}
