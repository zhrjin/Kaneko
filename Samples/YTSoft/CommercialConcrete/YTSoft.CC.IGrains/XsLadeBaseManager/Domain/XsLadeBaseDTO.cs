using AutoMapper;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.XsLadeBaseManager.Domain
{
	[AutoMap(typeof(XsLadeBaseVO))]
	public class XsLadeBaseDTO : BaseDTO<string>
	{
		#region 实体成员
		/// <summary>
		/// 
		/// </summary>
		public string XLB_LadeId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? XLB_SetDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Area { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Origin { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Line { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Client { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Cement { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_Price { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_CardPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_Total { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_FactNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_TurnNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_ReturnNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_FactTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_ScaleDifNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_InvoNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_SendArea { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_ApproveMan { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_CarCode { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public int? XLB_Quantity { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_PickMan { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public int? XLB_PrintNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_ScaleDifID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_IsOut { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Gather { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_IsInvo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Collate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Status { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Remark { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_ReturnRemark { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_IsAgainPrint { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Tranist { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_ColType { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? XLB_AuditCarryTime { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_TaskID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_BackNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_IsTransmit { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_OptName { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_OptID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_CarNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_AgiId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_Mortar { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_Method { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_ReturnPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_ReturnTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_AgentPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLB_AgentTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_RecipeNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLB_TurnType { set; get; }
		#endregion

		#region 扩展字段
		/// <summary>
		/// 
		/// </summary>
		public string StartTime { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string EndTime { set; get; }
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
		public Expression<Func<XsLadeBaseDO, bool>> GetExpression()
		{
			Expression<Func<XsLadeBaseDO, bool>> expression = null;

			if (!string.IsNullOrEmpty(XLB_LadeId))
			{
				Expression<Func<XsLadeBaseDO, bool>> exp = oo => oo.XLB_LadeId.Contains(XLB_LadeId);
				expression = expression == null ? exp : expression.And(exp);
			}
			if (!string.IsNullOrEmpty(XLB_CarCode))
			{
				Expression<Func<XsLadeBaseDO, bool>> exp = oo => oo.XLB_CarCode.Contains(XLB_CarCode);
				expression = expression == null ? exp : expression.And(exp);
			}
			if (Ids != null && Ids.Count > 0)
			{
				if (Ids.Count == 1)
				{
					var id = Ids[0];
					Expression<Func<XsLadeBaseDO, bool>> exp = oo => oo.Id == id;
					expression = expression == null ? exp : expression.And(exp);
				}
				else
				{
					Expression<Func<XsLadeBaseDO, bool>> exp = oo => Ids.Contains(oo.Id);
					expression = expression == null ? exp : expression.And(exp);
				}
			}
			if (!string.IsNullOrEmpty(StartTime))
			{
				DateTime dt = DateTime.Parse(StartTime);
				Expression<Func<XsLadeBaseDO, bool>> exp = oo => oo.XLB_SetDate >= dt;
				expression = expression == null ? exp : expression.And(exp);
			}
			if (!string.IsNullOrEmpty(EndTime))
			{
				DateTime dt = DateTime.Parse(EndTime);
				Expression<Func<XsLadeBaseDO, bool>> exp = oo => oo.XLB_SetDate <= dt;
				expression = expression == null ? exp : expression.And(exp);
			}

			return expression;
		}
		/// <summary>
		/// 搜索表达式
		/// </summary>
		/// <returns></returns>
		public Expression<Func<XsLadeBaseDO, bool>> GetExpression2()
		{
			Expression<Func<XsLadeBaseDO, bool>> expression = oo => oo.IsDel == 0;
			expression = expression.And(oo => oo.XLB_LadeId == XLB_LadeId);
			expression = expression.And(oo => oo.XLB_Firm == XLB_Firm);

			return expression;
		}
		#endregion
	}
}
