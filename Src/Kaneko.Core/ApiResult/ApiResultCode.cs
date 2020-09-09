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
        Success = 200,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 400,
        /// <summary>
        /// 异常
        /// </summary>
        Exception = 500,
        /// <summary>
        /// 没有登录信息
        /// </summary>
        Nologin = 410,

        /// <summary>
        /// 初始化密码
        /// </summary>
        Initpassword = 600,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownFail = 500,

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
