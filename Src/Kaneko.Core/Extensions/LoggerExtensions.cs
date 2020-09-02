using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Core.Extensions
{
    public static partial class LoggerExtensions
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void LogError(this ILogger logger, string message, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(ex, message);
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(message);
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(message);
            }
        }
    }
}
