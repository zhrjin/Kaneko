using System;
using System.Diagnostics;
using System.Reflection;

namespace Kaneko.Server.SkyAPM.Orleans.Diagnostic
{
    public static class DiagnosticExtension
    {
        public static long OrleansOnActivate(this DiagnosticListener listener, Type type, string grainId, string runtimeIdentity, string sw8)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansOnActivate))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    OperatioName = type?.Name,
                    GrainId = grainId,
                    GrainType = type?.FullName,
                    RuntimeIdentity = runtimeIdentity,
                    SW8 = sw8
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansOnActivate, excuteData);

                return now;
            }

            return 0;
        }

        public static long OrleansInvokeBefore(this DiagnosticListener listener, Type type, MethodInfo methodInfo, string grainId, string runtimeIdentity, string sw8)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeBefore))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    OperatioName = type?.Name,
                    GrainId = grainId,
                    GrainType = type?.FullName,
                    GrainMethod = methodInfo?.Name,
                    RuntimeIdentity = runtimeIdentity,
                    SW8 = sw8
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeBefore, excuteData);

                return now;
            }

            return 0;
        }

        public static void OrleansInvokeAfter(this DiagnosticListener listener, long tracingTimestamp)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeAfter))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    ElapsedTimeMs = now - tracingTimestamp,
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeAfter, excuteData);
            }
        }

        public static void OrleansInvokeError(this DiagnosticListener listener, long tracingTimestamp, Exception exception)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeError))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    ElapsedTimeMs = now - tracingTimestamp,
                    Exception = exception
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeError, excuteData);
            }
        }
    }
}
