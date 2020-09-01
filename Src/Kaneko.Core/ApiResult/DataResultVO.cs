using Kaneko.Core.Contract;
namespace Kaneko.Core.ApiResult
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
    }
}
