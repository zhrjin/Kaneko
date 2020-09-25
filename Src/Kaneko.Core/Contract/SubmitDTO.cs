namespace Kaneko.Core.Contract
{
    public class SubmitDTO<T> where T : IDataTransferObject
    {
        public T Data { set; get; }

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
