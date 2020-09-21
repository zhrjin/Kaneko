using System.Text.Json.Serialization;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 无数据，只结果标识
    /// </summary>

    public class ApiResult
    {
        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("code")]
        public ApiResultCode Code { set; get; }

        /// <summary>
        ///
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { set; get; }

        /// <summary>
        /// 成功标志
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success => Code == ApiResultCode.Success;
    }
}
