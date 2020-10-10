using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain
{
    public class XsCompyBaseDTO : BaseDTO<long>
    {
        #region 实体成员
        /// <summary>
        /// 客商主键
        /// </summary>
        public string XOB_ID { set; get; }

        /// <summary>
        /// 客商名称
        /// </summary>
        public string XOB_Name { set; get; }

        /// <summary>
        /// 对应企业
        /// </summary>
        public string XOB_Firm { set; get; }
        #endregion

        #region 扩展字段
        /// <summary>
        /// ID
        /// </summary>
        public List<string> Ids { set; get; } = new List<string>();
        #endregion

        #region 扩展操作
        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<XsCompyBaseDO, bool>> GetExpression()
        {
            Expression<Func<XsCompyBaseDO, bool>> expression = null;

            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Count == 1)
                {
                    var id = Ids[0];
                    Expression<Func<XsCompyBaseDO, bool>> exp = oo => oo.XOB_ID == id;
                    expression = expression == null ? exp : expression.And(exp);
                }
                else
                {
                    Expression<Func<XsCompyBaseDO, bool>> exp = oo => Ids.Contains(oo.XOB_ID);
                    expression = expression == null ? exp : expression.And(exp);
                }
            }
            if (!string.IsNullOrEmpty(XOB_ID))
            {
                Expression<Func<XsCompyBaseDO, bool>> exp = oo => oo.XOB_ID == XOB_ID;
                expression = expression == null ? exp : expression.And(exp);
            }

            return expression;
        }
        #endregion
    }
}
