using Kaneko.Dapper.Contract;

namespace Kaneko.Dapper.Result
{
    /// <summary>
    /// 单笔数据结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResultVO<TVO> : ResultVO where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
        public TVO Data { set; get; }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public void IsSuccess(TVO data, string message = "")
        {
            Message = message;
            Code = ResultCode.Succeed;
            Data = data ?? default;
        }
    }
}
