namespace Kaneko.Core.Contract
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public string Sidx { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string Sord { get; set; }
    }
}
