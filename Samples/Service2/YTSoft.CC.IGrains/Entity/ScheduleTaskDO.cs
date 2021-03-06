﻿using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.Entity
{
    [AutoMap(typeof(ScheduleTaskDTO))]
    [KanekoTable(name: "t_scheduletask333")]
    public class ScheduleTaskDO : SqlServerBaseDO<long>
    {
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

    public enum TaskState
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,

        /// <summary>
        /// 完成
        /// </summary>
        Complete = 2
    }
}
