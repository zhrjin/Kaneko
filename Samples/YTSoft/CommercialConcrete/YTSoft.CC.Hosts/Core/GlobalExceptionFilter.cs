using Kaneko.Core.ApiResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YTSoft.CC.Hosts.Core
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                ApiResult result = new ApiResult
                {
                    Code = ApiResultCode.Fail,
                    Info = context.Exception.Message
                };
                context.Result = new JsonResult(result);
            }
            context.ExceptionHandled = true;
        }
    }
}
