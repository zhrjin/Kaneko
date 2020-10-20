using System;
using System.Data.SqlClient;
using System.Linq;
using SkyApm;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace Kaneko.Server.SkyAPM.SqlClient
{
    public class SqlClientDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        private readonly ITracingContext _tracingContext;
        private readonly IExitSegmentContextAccessor _contextAccessor;

        public SqlClientDiagnosticProcessor(ITracingContext tracingContext,
            IExitSegmentContextAccessor contextAccessor)
        {
            _tracingContext = tracingContext;
            _contextAccessor = contextAccessor;
        }

        public string ListenerName { get; } = SqlClientDiagnosticStrings.DiagnosticListenerName;

        private static string ResolveOperationName(SqlCommand sqlCommand)
        {
            var commandType = sqlCommand.CommandText?.Split(' ');
            return $"{SqlClientDiagnosticStrings.SqlClientPrefix}{commandType?.FirstOrDefault()}";
        }

        [DiagnosticName(SqlClientDiagnosticStrings.SqlBeforeExecuteCommand)]
        public void BeforeExecuteCommand([Property(Name = "Command")] SqlCommand sqlCommand)
        {
            var context = _tracingContext.CreateExitSegmentContext(ResolveOperationName(sqlCommand),
                sqlCommand.Connection.DataSource);
            context.Span.SpanLayer = SkyApm.Tracing.Segments.SpanLayer.DB;
            context.Span.Component = SkyApm.Common.Components.SQLCLIENT;
            context.Span.AddTag(SkyApm.Common.Tags.DB_TYPE, "sql");
            context.Span.AddTag(SkyApm.Common.Tags.DB_INSTANCE, sqlCommand.Connection.Database);
            context.Span.AddTag(SkyApm.Common.Tags.DB_STATEMENT, sqlCommand.CommandText);
            context.Span.AddLog(LogEvent.Event("Sql Invoke Begin"));
        }

        [DiagnosticName(SqlClientDiagnosticStrings.SqlAfterExecuteCommand)]
        public void AfterExecuteCommand()
        {
            var context = _contextAccessor.Context;
            if (context != null)
            {
                context.Span.AddLog(LogEvent.Event("Sql Invoke End"));
                _tracingContext.Release(context);
            }
        }

        [DiagnosticName(SqlClientDiagnosticStrings.SqlErrorExecuteCommand)]
        public void ErrorExecuteCommand([Property(Name = "Exception")] Exception ex)
        {
            var context = _contextAccessor.Context;
            if (context != null)
            {
                context.Span.AddLog(LogEvent.Event("Sql Invoke Error"));
                context.Span.ErrorOccurred(ex);
                _tracingContext.Release(context);
            }
        }
    }
}
