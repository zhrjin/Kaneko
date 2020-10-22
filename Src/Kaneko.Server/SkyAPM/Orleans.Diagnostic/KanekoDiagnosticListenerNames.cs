namespace Kaneko.Server.SkyAPM.Orleans.Diagnostic
{
    public static class KanekoDiagnosticListenerNames
    {
        private const string KanekoPrefix = "Kaneko.SOA.";

        public const string OperationId = "KanekoDiagnosticOperationId";

        public const string DiagnosticListenerName = "KanekoDiagnosticListener";

        public const string ControllerExecuteBefore = KanekoPrefix + "ControllerExecuteBefore";
        public const string ControllerExecuteAfter = KanekoPrefix + "ControllerExecuteAfter";
        public const string OrleansInvokeBefore = KanekoPrefix + "OrleansInvokeBefore";
        public const string OrleansInvokeAfter = KanekoPrefix + "OrleansInvokeAfter";
        public const string OrleansInvokeError = KanekoPrefix + "OrleansInvokeError";
        public const string OrleansOnActivate = KanekoPrefix + "OrleansOnActivate";

    }
}
