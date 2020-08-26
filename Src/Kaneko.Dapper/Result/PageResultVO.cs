using Kaneko.Dapper.Contract;

namespace Kaneko.Dapper.Result
{
    /// <summary>
    /// 分页列表数据结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResultVO<TVO> : ListResultVO<TVO> where TVO : IViewObject
    {
        /// <summary>
        /// 笔数
        /// </summary>
        public int Count { set; get; }
    }
}
