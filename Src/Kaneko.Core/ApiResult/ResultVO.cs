namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 无数据，只结果标识
    /// </summary>
    public class ResultVO
    {
        /// <summary>
        ///
        /// </summary>
        public ResultCode Code { set; get; }

        /// <summary>
        ///
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// 成功标志
        /// </summary>
        public bool Success => Code == ResultCode.Succeed;
    }
}
