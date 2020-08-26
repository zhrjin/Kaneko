using Kaneko.Dapper.Contract;
using System.Collections.Generic;

namespace Kaneko.Dapper.Result
{
    /// <summary>
    /// 列表数据结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResultVO<TVO> : ResultVO where TVO : IViewObject
    {
        /// <summary>
        /// 数据
        /// </summary>
        public IList<TVO> Data { set; get; }
    }
}
