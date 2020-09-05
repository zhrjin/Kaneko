namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 异常码
    /// </summary>
    public enum ApiResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 200,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownFail = 500,

        /// <summary>
        /// 程序异常
        /// </summary>
        Error = 501,

        /// <summary>
        /// 未发现
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 无权限
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 访问被禁止
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 请求的格式不对
        /// </summary>
        NotAcceptable = 406,

        /// <summary>
        /// 参数校验错误
        /// </summary>
        ArgumentError = 422
    }
}
