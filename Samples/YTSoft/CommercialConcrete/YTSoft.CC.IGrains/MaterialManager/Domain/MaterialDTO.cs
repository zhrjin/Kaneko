using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.MaterialManager.Domain
{
    public class MaterialDTO : BaseDTO<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public List<string> Ids { set; get; } = new List<string>();

        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<MaterialDO, bool>> GetExpression()
        {
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Count == 1)
                {
                    var id = Ids[0];
                    Expression<Func<MaterialDO, bool>> expression = oo => oo.Id == id;
                    return expression;
                }
                else
                {
                    Expression<Func<MaterialDO, bool>> expression = oo => Ids.Contains(oo.Id);
                    return expression;
                }
            }

            return null;
        }
    }
}
