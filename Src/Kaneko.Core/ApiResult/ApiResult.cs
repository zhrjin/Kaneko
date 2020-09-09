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
        public ApiResultCode Code { set; get; }

        /// <summary>
        ///
        /// </summary>
        public string Info { set; get; }

        /// <summary>
        /// 成功标志
        /// </summary>
        public bool Success => Code == ApiResultCode.Success;
    }
}
