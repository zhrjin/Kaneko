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
        public static ResultVO IsSuccess(string message = "")
        {
            return new ResultVO()
            {
                Message = message,
                Code = ResultCode.Succeed
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultVO IsFailed(string message = "")
        {
            return new ResultVO()
            {
                Message = message,
                Code = ResultCode.UnknownFail
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        public static ResultVO IsFailed(ResultCode resultCode, string message = "")
        {
            return new ResultVO()
            {
                Message = message,
                Code = resultCode
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="exexception></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultVO IsFailed(Exception exception)
        {
            return new ResultVO()
            {
                Message = exception.InnerException?.StackTrace,
                Code = ResultCode.Error
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="exception"></param>
        public static ResultVO IsFailed(ResultCode resultCode, Exception exception)
        {
            return new ResultVO()
            {
                Message = exception.InnerException?.StackTrace,
                Code = resultCode
            };
        }

        /// <summary>
        /// 无数据
        /// </summary>
        /// <param name="message"></param>
        public static ResultVO IsNotFound(string message = "")
        {
            return new ResultVO()
            {
                Message = string.IsNullOrEmpty(message) ? "无数据" : message,
                Code = ResultCode.NotFound
            };
        }

        /// <summary>
        /// 无权限
        /// </summary>
        /// <param name="message"></param>
        public static ResultVO IsUnauthorized(string message = "")
        {
            return new ResultVO()
            {
                Message = string.IsNullOrEmpty(message) ? "无权限" : message,
                Code = ResultCode.Unauthorized
            };
        }

        /// <summary>
        /// 访问被禁止
        /// </summary>
        /// <param name="message"></param>
        public static ResultVO IsForbidden(string message = "")
        {
            return new ResultVO()
            {
                Message = string.IsNullOrEmpty(message) ? "访问被禁止" : message,
                Code = ResultCode.Forbidden
            };
        }

        /// <summary>
        /// 参数校验错误
        /// </summary>
        /// <param name="message"></param>
        public static ResultVO IsArgumentError(string message = "")
        {
            return new ResultVO()
            {
                Message = string.IsNullOrEmpty(message) ? "访问被禁止" : message,
                Code = ResultCode.ArgumentError
            };
        }

        /// <summary>
        /// 请求的格式不对
        /// </summary>
        /// <param name="message"></param>
        public static ResultVO IsNotAcceptable(string message = "")
        {
            return new ResultVO()
            {
                Message = string.IsNullOrEmpty(message) ? "访问被禁止" : message,
                Code = ResultCode.NotAcceptable
            };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public static DataResultVO<TVO> IsSuccess<TVO>(TVO data, string message = "") where TVO : IViewObject
        {
            return new DataResultVO<TVO>()
            {
                Message = message,
                Code = ResultCode.Succeed,
                Data = data ?? default
            };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public static ListResultVO<TVO> IsSuccess<TVO>(IList<TVO> data, string message = "") where TVO : IViewObject
        {
            return new ListResultVO<TVO>()
            {
                Message = message,
                Code = ResultCode.Succeed,
                Data = data ?? default
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
        public static PageResultVO<TVO> IsSuccess<TVO>(IList<TVO> data, int count, string message = "") where TVO : IViewObject
        {
            return new PageResultVO<TVO>()
            {
                Message = message,
                Code = ResultCode.Succeed,
                Data = data ?? default,
                Count = count
            };
        }
    }
}
