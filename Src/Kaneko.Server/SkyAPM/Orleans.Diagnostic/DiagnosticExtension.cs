using System;
using System.Diagnostics;
using System.Reflection;

namespace Kaneko.Server.SkyAPM.Orleans.Diagnostic
{
    public static class DiagnosticExtension
    {
        public static long? OrleansInvokeBefore(this DiagnosticListener listener, Type type, MethodInfo methodInfo, string grainId, string runtimeIdentity, string sw8)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeBefore))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    OperatioName = type?.Name + "." + methodInfo?.Name,
                    GrainId = grainId,
                    GrainType = type?.FullName,
                    GrainMethod = methodInfo?.Name,
                    OperationTimestamp = now,
                    RuntimeIdentity = runtimeIdentity,
                    SW8 = sw8
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeBefore, excuteData);

                return now;
            }

            return null;
        }

        public static void OrleansInvokeAfter(this DiagnosticListener listener, long? tracingTimestamp, Type type, MethodInfo methodInfo,
            string grainId, string runtimeIdentity)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeAfter))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    GrainId = grainId,
                    GrainType = type?.FullName,
                    GrainMethod = methodInfo?.Name,
                    OperatioName = type?.Name + "." + methodInfo?.Name,
                    ElapsedTimeMs = now - tracingTimestamp.Value,
                    RuntimeIdentity = runtimeIdentity
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeAfter, excuteData);
            }
        }

        public static void OrleansInvokeError(this DiagnosticListener listener, long? tracingTimestamp, Type type, MethodInfo methodInfo, string grainId, string runtimeIdentity, Exception exception)
        {
            if (listener.IsEnabled(KanekoDiagnosticListenerNames.OrleansInvokeError))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var excuteData = new KanekoExcuteData
                {
                    GrainId = grainId,
                    GrainType = type?.FullName,
                    GrainMethod = methodInfo?.Name,
                    OperatioName = type?.Name + "." + methodInfo?.Name,
                    Exception = exception,
                    ElapsedTimeMs = now - tracingTimestamp.Value,
                    RuntimeIdentity = runtimeIdentity
                };
                listener.Write(KanekoDiagnosticListenerNames.OrleansInvokeError, excuteData);
            }
        }
    }
}
