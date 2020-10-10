namespace TYSoft.Common.Model.EventBus
{
    public class EventContract
    {
        /// <summary>
        /// 商砼服务
        /// </summary>
        public class ComConcrete
        {
            /// <summary>
            /// 任务单同步到中控平台
            /// </summary>
            public const string TaskListToZK = "ComConcrete.TaskListToZK";

            /// <summary>
            /// 
            /// </summary>
            public const string WithTransactionTest = "ComConcrete.WithTransactionTest";
        }

        public class Tester
        {
            /// <summary>
            /// 测试
            /// </summary>
            public const string Test = "Tester.Test";
        }
    }
}
