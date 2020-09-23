namespace Kaneko.Core.ApiResult
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
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Records { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Total
        {
            get
            {
                if (Records > 0)
                {
                    return Records % this.Rows == 0 ? Records / this.Rows : Records / this.Rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
