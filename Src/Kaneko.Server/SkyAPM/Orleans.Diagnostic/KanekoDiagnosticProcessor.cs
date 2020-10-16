using SkyApm;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Common;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System.Collections.Generic;

namespace Kaneko.Server.SkyAPM.Orleans.Diagnostic
{
    /// <summary>
    ///  
    /// </summary>
    public class KanekoDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public string ListenerName => KanekoDiagnosticListenerNames.DiagnosticListenerName;

        private readonly ITracingContext _tracingContext;
        private readonly IEntrySegmentContextAccessor _entrySegmentContextAccessor;
        private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;
        private readonly IExitSegmentContextAccessor _exitSegmentContextAccessor;

        public KanekoDiagnosticProcessor(ITracingContext tracingContext,
            IEntrySegmentContextAccessor entrySegmentContextAccessor,
            ILocalSegmentContextAccessor localSegmentContextAccessor,
            IExitSegmentContextAccessor exitSegmentContextAccessor
            )
        {
            _tracingContext = tracingContext;
            _entrySegmentContextAccessor = entrySegmentContextAccessor;
            _localSegmentContextAccessor = localSegmentContextAccessor;
            _exitSegmentContextAccessor = exitSegmentContextAccessor;
        }


        [DiagnosticName(KanekoDiagnosticListenerNames.OrleansInvokeBefore)]
        public void OrleansInvokeBefore([Object] KanekoExcuteData eventData)
        {
            //var context = _tracingContext.CreateLocalSegmentContext(eventData.OperatioName);

            //var context = _tracingContext.CreateExitSegmentContext(eventData.OperatioName, eventData.GetRuntimeIdentity(), eventData.HttpRequestHeader);

            var context = _tracingContext.CreateEntrySegmentContext(eventData.OperatioName, new HttpRequestCarrierHeaderCollection(eventData.SW8));

            context.Span.Component = GetComponent();
            context.Span.Peer = new StringOrIntValue(eventData.GetRuntimeIdentity());
            context.Span.AddTag("grain.instance", eventData.GrainType);
            context.Span.AddTag("grain.method", eventData.GrainMethod);
            context.Span.AddTag("grain.identity", eventData.GrainId);
            context.Span.AddLog(
              LogEvent.Event("Grain Invoke Begin"),
              LogEvent.Message(
                  $"Grain starting"));

        }

        [DiagnosticName(KanekoDiagnosticListenerNames.OrleansInvokeAfter)]
        public void OrleansInvokeAfter([Object] KanekoExcuteData eventData)
        {
            var context = _entrySegmentContextAccessor.Context;
            if (context == null) return;

            context.Span.AddTag("grain.instance", eventData.GrainType);
            context.Span.AddTag("grain.method", eventData.GrainMethod);
            context.Span.AddTag("grain.identity", eventData.GrainId);
            context.Span.AddLog(
              LogEvent.Event("Grain Invoke End"),
              LogEvent.Message(
                  $"Grain finished spend time: " + eventData.ElapsedTimeMs + "ms"));

            _tracingContext.Release(context);
        }

        [DiagnosticName(KanekoDiagnosticListenerNames.OrleansInvokeError)]
        public void OrleansInvokeError([Object] KanekoExcuteData eventData)
        {
            var context = _entrySegmentContextAccessor.Context;
            if (context == null) return;

            context.Span.AddTag("grain.instance", eventData.GrainType);
            context.Span.AddTag("grain.method", eventData.GrainMethod);
            context.Span.AddTag("grain.identity", eventData.GrainId);
            context.Span.AddLog(
              LogEvent.Event("Grain Invoke Error"),
              LogEvent.Message(
                  $"Grain Error spend time: " + eventData.ElapsedTimeMs + "ms"));
            context.Span?.ErrorOccurred(eventData.Exception);
            _tracingContext.Release(context);
        }

        private StringOrIntValue GetComponent()
        {
            return new StringOrIntValue(3017, "Orleans");
        }
    }
}