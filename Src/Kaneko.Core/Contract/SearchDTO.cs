namespace Kaneko.Core.Contract
{
    public class SearchDTO
    {
        public string Pagination { set; get; }
        public string QueryJson { set; get; }
    }

    public class SearchDTO<T> where T : IDataTransferObject
    {
        public T Data { set; get; }

        /// <summary>
        /// 页标
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页笔数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
