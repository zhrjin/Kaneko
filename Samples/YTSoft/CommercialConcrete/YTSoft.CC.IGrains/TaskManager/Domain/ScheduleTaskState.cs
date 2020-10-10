using AutoMapper;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.TaskManager.Domain
{
    [AutoMap(typeof(ScheduleTaskDO))]
    public class ScheduleTaskState : BaseState<long>
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskCode { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { set; get; }

        /// <summary>
        /// 生产线编号
        /// </summary>
        public string LineCode { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public TaskState TaskState { set; get; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ItemNumber { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { set; get; }
    }
}
