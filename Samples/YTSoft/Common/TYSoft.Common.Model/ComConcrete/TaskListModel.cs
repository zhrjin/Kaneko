using Kaneko.Core.Attributes;
using TYSoft.Common.Model.EventBus;

namespace TYSoft.Common.Model.ComConcrete
{
    /// <summary>
    /// 任务模型
    /// </summary>
    [EventName(EventContract.ComConcrete.WithTransactionTest)]
    public class TaskListModel
    {
        public string TaskCode { set; get; }

        public string TaskName { set; get; }

        /// <summary>
        /// 批次号，用于防重复同步
        /// </summary>
        public string BatchNo { set; get; }
    }
}
