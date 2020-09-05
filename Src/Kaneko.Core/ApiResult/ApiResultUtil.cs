using Kaneko.Core.Contract;
using System;
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
                Message = message,
                Code = ApiResultCode.Succeed
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
                Message = message,
                Code = ApiResultCode.Succeed,
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
                Message = message,
                Code = ApiResultCode.Succeed,
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
                Message = message,
                Code = ApiResultCode.Succeed,
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
        public static ApiResult IsFailed(string message = "", ApiResultCode resultCode = ApiResultCode.UnknownFail)
        {
            return new ApiResult()
            {
                Message = message,
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
        public static ApiResult<TVO> IsFailed<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.UnknownFail) where TVO : IViewObject
        {
            return new ApiResult<TVO>()
            {
                Message = message,
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
        public static ApiResultList<TVO> IsFailedList<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.UnknownFail) where TVO : IViewObject
        {
            return new ApiResultList<TVO>()
            {
                Message = message,
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
        public static ApiResultPage<TVO> IsFailedPage<TVO>(string message = "", ApiResultCode resultCode = ApiResultCode.UnknownFail) where TVO : IViewObject
        {
            return new ApiResultPage<TVO>()
            {
                Message = message,
                Code = resultCode,
                Data = default
            };
        }
    }
}
