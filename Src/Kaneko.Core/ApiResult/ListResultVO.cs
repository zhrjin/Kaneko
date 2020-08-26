using Kaneko.Core.Contract;
using System.Collections.Generic;

namespace Kaneko.Core.ApiResult
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
