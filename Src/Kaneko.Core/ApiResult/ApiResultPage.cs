using Kaneko.Core.Contract;

namespace Kaneko.Core.ApiResult
{
    /// <summary>
    /// 分页列表数据结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultPage<TVO> : ApiResultList<TVO> where TVO : IViewObject
    {
        /// <summary>
        /// 笔数
        /// </summary>
        public int Count { set; get; }
    }
}
