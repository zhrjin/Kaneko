using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain
{
    public class PbCodeMaterialDTO : BaseDTO<long>
    {
        #region 实体成员
        /// <summary>
        /// 物料主键
        /// </summary>
        public string PCM_ID { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string PCM_Name { set; get; }

        /// <summary>
        /// 对应企业
        /// </summary>
        public string PCM_Firm { set; get; }
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
        public Expression<Func<PbCodeMaterialDO, bool>> GetExpression()
        {
            Expression<Func<PbCodeMaterialDO, bool>> expression = null;

            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Count == 1)
                {
                    var id = Ids[0];
                    Expression<Func<PbCodeMaterialDO, bool>> exp = oo => oo.PCM_ID == id;
                    expression = expression == null ? exp : expression.And(exp);
                }
                else
                {
                    Expression<Func<PbCodeMaterialDO, bool>> exp = oo => Ids.Contains(oo.PCM_ID);
                    expression = expression == null ? exp : expression.And(exp);
                }
            }
            if (!string.IsNullOrEmpty(PCM_ID))
            {
                Expression<Func<PbCodeMaterialDO, bool>> exp = oo => oo.PCM_ID == PCM_ID;
                expression = expression == null ? exp : expression.And(exp);
            }

            return expression;
        }
        #endregion
    }
}
