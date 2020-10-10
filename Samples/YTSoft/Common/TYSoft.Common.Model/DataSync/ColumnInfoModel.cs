namespace TYSoft.Common.Model.DataSync
{
    public class ColumnInfoModel
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
    }
}
