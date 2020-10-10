using System.Collections.Generic;

namespace TYSoft.Common.Model.DataSync
{
    public class TableInfoModel
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
        public SystemInfoModel SystemInfo { set; get; }

        /// <summary>
        /// 列
        /// </summary>
        public List<ColumnInfoModel> Columns { set; get; }

    }
}
