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

        public const string BeforePublish = KanekoPrefix + "WritePublishBefore";
        public const string AfterPublish = KanekoPrefix + "WritePublishAfter";
        public const string ErrorPublish = KanekoPrefix + "WritePublishError";

        public const string BeforeConsume = KanekoPrefix + "WriteConsumeBefore";
        public const string AfterConsume = KanekoPrefix + "WriteConsumeAfter";
        public const string ErrorConsume = KanekoPrefix + "WriteConsumeError";

        public const string BeforeSubscriberInvoke = KanekoPrefix + "WriteSubscriberInvokeBefore";
        public const string AfterSubscriberInvoke = KanekoPrefix + "WriteSubscriberInvokeAfter";
        public const string ErrorSubscriberInvoke = KanekoPrefix + "WriteSubscriberInvokeError";
    }
}
