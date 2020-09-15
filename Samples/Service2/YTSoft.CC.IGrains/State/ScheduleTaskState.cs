using AutoMapper;
using Kaneko.Core.Contract;
using YTSoft.CC.IGrains.Entity;

namespace YTSoft.CC.IGrains.State
{
    [AutoMap(typeof(ScheduleTaskDO))]
    public class ScheduleTaskState : BsseState<long>
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
    }
}
