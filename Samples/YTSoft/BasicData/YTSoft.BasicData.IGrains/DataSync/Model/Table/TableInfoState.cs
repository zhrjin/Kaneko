using AutoMapper;
using System;
using System.Collections.Generic;
using YTSoft.BasicData.IGrains.DataDictionary.Model;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    [AutoMap(typeof(TableInfoDO))]
    [Serializable]
    public class TableInfoState
    {
        /// <summary>
        /// 表ID
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 组别
        /// </summary>
        public string GroupCode { set; get; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 1-已删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public SystemInfoState SystemInfo { set; get; } = new SystemInfoState();

        /// <summary>
        /// 列
        /// </summary>
        public List<ColumnInfoState> Columns { set; get; } = new List<ColumnInfoState>();
    }
}
