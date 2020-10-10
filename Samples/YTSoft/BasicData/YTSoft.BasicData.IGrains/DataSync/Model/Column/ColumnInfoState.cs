using AutoMapper;
using System;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    /// <summary>
    /// 列配置
    /// </summary>
    [AutoMap(typeof(ColumnInfoDO))]
    [Serializable]
    public class ColumnInfoState
    {
        /// <summary>
        /// 自身列名称
        /// </summary>
        public string SelfColumn { set; get; }

        /// <summary>
        /// 模型列名称
        /// </summary>
        public string ModelColumn { set; get; }

        /// <summary>
        /// 1-已删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int SortNo { set; get; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string DisplayName { set; get; }
    }
}
