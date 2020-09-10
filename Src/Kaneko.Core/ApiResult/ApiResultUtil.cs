using Kaneko.Core.Contract;
using System.Collections.Generic;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 结果集
    /// </summary>
    public static class ApiResultUtil
    {
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult IsSuccess(string message = "")
        {
            return new ApiResult()
            {
                Info = message,
                Code = ApiResultCode.Success
            };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public static ApiResult<TVO> IsSuccess<TVO>(TVO data, string message = "") where TVO : IViewObject
        {
            return new ApiResult<TVO>()
            {
                Info = message,
                Code = ApiResultCode.Success,
                Data = data ?? default
            };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <typeparam name="TVO"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResultList<TVO> IsSuccess<TVO>(IList<TVO> data, string message = "") where TVO : IViewObject
        {
            return new ApiResultPage<TVO>()
            {
                Info = message,
                Code = ApiResultCode.Success,
                Data = data ?? default,
            };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <typeparam name="TVO"></typeparam>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResultPage<TVO> IsSuccess<TVO>(IList<TVO> data, int count, string message = "") where TVO : IViewObject
        {
            return new ApiResultPage<TVO>()
            {
                Info = message,
                Code = ApiResultCode.Success,
                Data = data ?? default,
                Count = count
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static ApiResult IsFailed(string message = "", ApiResultCode resultCode = ApiResultCode.Fail)
        {
            return new ApiResult()
            {
                Info = message,
                Code = resultCode
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <typeparam name="TVO"></typeparam>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static ApiResult<TVO> IsFailed<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.Fail) where TVO : IViewObject
        {
            return new ApiResult<TVO>()
            {
                Info = message,
                Code = resultCode,
                Data = default
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <typeparam name="TVO"></typeparam>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static ApiResultList<TVO> IsFailedList<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.Fail) where TVO : IViewObject
        {
            return new ApiResultList<TVO>()
            {
                Info = message,
                Code = resultCode,
                Data = default
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <typeparam name="TVO"></typeparam>
        /// <param name="message"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static ApiResultPage<TVO> IsFailedPage<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.Fail) where TVO : IViewObject
        {
            return new ApiResultPage<TVO>()
            {
                Info = message,
                Code = resultCode,
                Data = default
            };
        }
    }
}
