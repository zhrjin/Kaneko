using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain
{
    public class PbBasicFirmDTO : BaseDTO<long>
    {
        #region 实体成员
        /// <summary>
        /// 企业主键
        /// </summary>
        public string PBF_ID { set; get; }

        /// <summary>
        /// 企业简称
        /// </summary>
        public string PBF_ShortName { set; get; }
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
        public Expression<Func<PbBasicFirmDO, bool>> GetExpression()
        {
            Expression<Func<PbBasicFirmDO, bool>> expression = null;

            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Count == 1)
                {
                    var id = Ids[0];
                    Expression<Func<PbBasicFirmDO, bool>> exp = oo => oo.PBF_ID == id;
                    expression = expression == null ? exp : expression.And(exp);
                }
                else
                {
                    Expression<Func<PbBasicFirmDO, bool>> exp = oo => Ids.Contains(oo.PBF_ID);
                    expression = expression == null ? exp : expression.And(exp);
                }
            }
            if (!string.IsNullOrEmpty(PBF_ID))
            {
                Expression<Func<PbBasicFirmDO, bool>> exp = oo => oo.PBF_ID == PBF_ID;
                expression = expression == null ? exp : expression.And(exp);
            }

            return expression;
        }
        #endregion
    }
}
