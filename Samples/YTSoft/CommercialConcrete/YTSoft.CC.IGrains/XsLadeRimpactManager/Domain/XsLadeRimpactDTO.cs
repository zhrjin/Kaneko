using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager.Domain
{
	public class XsLadeRimpactDTO : BaseDTO<string>
	{
		#region 实体成员
		/// <summary>
		/// 
		/// </summary>
		public string XLR_Lade { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? XLR_SetDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLR_Type { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLR_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XLR_Total { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLR_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XLR_Remark { set; get; }
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
		/// 
		/// </summary>
		public string XLB_LadeId { get; set; }
		/// <summary>
		/// ID
		/// </summary>
		public List<string> XLR_Lades { set; get; } = new List<string>();
		#endregion

		#region 扩展操作
		/// <summary>
		/// 搜索表达式
		/// </summary>
		/// <returns></returns>
		public Expression<Func<XsLadeRimpactDO, bool>> GetExpression()
		{
			Expression<Func<XsLadeRimpactDO, bool>> expression = oo => oo.IsDel == 0;

			if (!string.IsNullOrEmpty(XLR_Firm))
				expression = expression.And(oo => oo.XLR_Firm == XLR_Firm);
			if (!string.IsNullOrEmpty(StartTime))
			{
				DateTime dt = DateTime.Parse(StartTime);
				expression = expression.And(oo => oo.XLR_SetDate >= dt);
			}
			if (!string.IsNullOrEmpty(EndTime))
			{
				DateTime dt = DateTime.Parse(EndTime);
				expression = expression.And(oo => oo.XLR_SetDate <= dt);
			}
			if (XLR_Lades != null && XLR_Lades.Count > 0)
			{
				if (XLR_Lades.Count == 1)
				{
					var XLR_Lade = XLR_Lades[0];
					expression = expression.And(oo => oo.XLR_Lade == XLR_Lade);
					return expression;
				}
				else
				{
					expression = expression.And(oo => XLR_Lades.Contains(oo.XLR_Lade));
					return expression;
				}
			}

			return expression;
		}
		#endregion
	}
}
