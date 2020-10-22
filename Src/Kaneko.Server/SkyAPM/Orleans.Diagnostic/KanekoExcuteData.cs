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

        public string RuntimeIdentity { set; private get; }

        public Exception Exception { get; set; }

        public string SW8 { get; set; }

        public string GetRuntimeIdentity()
        {
            if (!string.IsNullOrEmpty(RuntimeIdentity))
            {
                string[] arr = RuntimeIdentity.Split(new char[1] { ':' });
                if (arr.Length >= 3)
                {
                    return arr[0].Replace("S", "") + ":" + arr[1];
                }
            }
            return RuntimeIdentity;
        }
    }
}
