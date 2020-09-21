using AutoMapper;
using Kaneko.Core.Contract;
using YTSoft.CC.IGrains.Entity;
using YTSoft.CC.IGrains.State;

namespace YTSoft.CC.IGrains.VO
{
    [AutoMap(typeof(ScheduleTaskDO))]
    [AutoMap(typeof(ScheduleTaskState))]
    public class ScheduleTaskVO : BaseVO<long>
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

        public int? ddddd { set; get; }
    }
}
