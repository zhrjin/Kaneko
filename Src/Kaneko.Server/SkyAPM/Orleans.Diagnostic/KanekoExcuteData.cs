using System;

namespace Kaneko.Server.SkyAPM.Orleans.Diagnostic
{
    public class KanekoExcuteData
    {
        public string GrainId { get; set; }

        public string OperatioName { get; set; }

        public string GrainType { get; set; }

        public string GrainMethod { get; set; }

        /// <summary>
        /// 消耗时间
        /// </summary>
        public long? ElapsedTimeMs { get; set; }

        public long? OperationTimestamp { get; set; }

        public Exception Exception { get; set; }
    }
}
