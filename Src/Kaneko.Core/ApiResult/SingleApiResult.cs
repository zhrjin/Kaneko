using Kaneko.Core.Contract;
using System.Text.Json.Serialization;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 单笔数据结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<TVO> : ApiResult //where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public TVO Data { set; get; }
    }
}
