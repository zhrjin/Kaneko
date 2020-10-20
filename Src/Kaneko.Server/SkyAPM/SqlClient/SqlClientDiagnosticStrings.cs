using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Server.SkyAPM.SqlClient
{
    internal static class SqlClientDiagnosticStrings
    {
        public const string DiagnosticListenerName = "SqlClientDiagnosticListener";

        public const string SqlClientPrefix = "sqlClient ";

        public const string SqlBeforeExecuteCommand = "System.Data.SqlClient.WriteCommandBefore";
        public const string SqlAfterExecuteCommand = "System.Data.SqlClient.WriteCommandAfter";
        public const string SqlErrorExecuteCommand = "System.Data.SqlClient.WriteCommandError";
    }
}
