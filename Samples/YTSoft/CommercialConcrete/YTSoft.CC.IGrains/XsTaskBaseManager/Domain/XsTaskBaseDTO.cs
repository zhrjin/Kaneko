using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;


namespace YTSoft.CC.IGrains.XsTaskBaseManager.Domain
{
    public class XsTaskBaseDTO : BaseDTO<string>
    {
        public string XTB_Code { set; get; }

        public DateTime? XTB_SetDate { set; get; }

        public string XTB_Area { get; set; }

        public string XTB_Contract { get; set; }

        public string XTB_InDent { get; set; }

        public string XTB_TaskType { get; set; }

        public string XTB_Client { get; set; }

        public string XTB_ProName { get; set; }

        public string XTB_ArName { get; set; }

        public string XTB_ArSpace { get; set; }

        public string XTB_Location { get; set; }

        public string XTB_Cement { get; set; }

        public string XTB_Auxiliary { get; set; }

        public string XTB_Pumper { get; set; }

        public string XTB_Salesman { get; set; }

        public decimal? XTB_Number { get; set; }

        public decimal? XTB_Price { get; set; }

        public decimal? XTB_TotalPrice { get; set; }

        public decimal? XTB_Total { get; set; }

        public DateTime? XTB_BegDate { get; set; }

        public DateTime? XTB_EndDate { get; set; }

        public string XTB_Casting { get; set; }

        public string XTB_Slumps { get; set; }

        public string XTB_Maximum { get; set; }

        public string XTB_LinkMan { get; set; }

        public string XTB_LinkTel { get; set; }

        public string XTB_SendState { get; set; }

        public string XTB_Remark { get; set; }

        public string XTB_AuditFlow { get; set; }

        public string XTB_AuditTache { get; set; }

        public string XTB_AuditState { get; set; }

        public string XTB_IsStop { get; set; }

        public int? XTB_PrnTimes { get; set; }

        public string XTB_Status { get; set; }

        public string XTB_Firm { get; set; }

        public string XTB_ProId { get; set; }

        public string XTB_Method { get; set; }

        public string XTB_TaskName { get; set; }

        public decimal? XTB_Machine { get; set; }

        public decimal? XTB_PumperPrice { get; set; }

        public decimal? XTB_AllPumper { get; set; }

        public decimal? XTB_FrePrice { get; set; }

        public string XTB_IsSend { get; set; }

        public string XTB_SendUser { get; set; }

        public DateTime? XTB_SendDate { get; set; }

        public string XTB_IsCancel { get; set; }

        public string XTB_CancelUser { get; set; }

        public DateTime? XTB_CancelDate { get; set; }

        public string XTB_Matching { get; set; }

        public string XTB_IsSupply { get; set; }

        public string XTB_Formula { get; set; }

        #region 扩展字段
        public string StartTime { get; set; }

        public string EndTime { get; set; }
        #endregion



        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(XTB_Code))
                yield return new ValidationResult("任务单编号不能为空！", new[] { nameof(XTB_Code) });
        }
        public Expression<Func<XsTaskBaseDO, bool>> GetPageExpression()
        {
            Expression<Func<XsTaskBaseDO, bool>> expression = oo => oo.IsDel == 0;

            if (!string.IsNullOrEmpty(XTB_Code))
                expression = expression.And(oo => oo.XTB_Code.Contains(XTB_Code));
            if (!string.IsNullOrEmpty(XTB_Client))
                expression = expression.And(oo => oo.XTB_Client.Contains(XTB_Client));
            if (!string.IsNullOrEmpty(StartTime))
            {
                DateTime dt = DateTime.Parse(StartTime);
                expression = expression.And(oo => oo.XTB_SetDate >= dt);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                DateTime dt = DateTime.Parse(EndTime);
                expression = expression.And(oo => oo.XTB_SetDate <= dt);
            }

            return expression;
        }
        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<XsTaskBaseDO, bool>> GetExpression()
        {
            Expression<Func<XsTaskBaseDO, bool>> expression = oo => oo.IsDel == 0;

            if (!string.IsNullOrEmpty(XTB_Code))
                expression = expression.And(oo => oo.XTB_Code.Contains(XTB_Code));

            return expression;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public override OrderByField[] GetOrder()
        {
            var list = new List<OrderByField>()
            {
               OrderByField.Create<XsTaskBaseDO>(nameof(XsTaskBaseDO.CreateDate), FieldSortType.Desc)
            };
            return list.ToArray();
        }
    }
}
