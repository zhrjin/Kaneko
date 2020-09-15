using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.Entity
{
    [AutoMap(typeof(ScheduleTaskDTO))]
    [KanekoTable(name: "t_scheduletask17")]
    public class ScheduleTaskDO17 : BaseDO
    {
        /// <summary>
        /// 任务ID，主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "id", ColumnDefinition = "varchar(20) not null primary key")]
        public string Id { set; get; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [KanekoColumn(Name = "task_code", ColumnDefinition = "varchar(40) null")]
        public string TaskCode { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [KanekoColumn(Name = "task_name", ColumnDefinition = "varchar(255) null")]
        public string TaskName { set; get; }

        /// <summary>
        /// 生产线编号
        /// </summary>
        [KanekoColumn(Name = "line_code", ColumnDefinition = "varchar(255) null")]
        public string LineCode { set; get; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [KanekoColumn(Name = "task_state", ColumnDefinition = "int null")]
        public TaskState TaskState { set; get; }
    }

}
