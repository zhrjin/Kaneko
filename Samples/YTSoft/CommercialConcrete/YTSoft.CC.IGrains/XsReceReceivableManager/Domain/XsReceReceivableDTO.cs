using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.XsReceReceivableManager.Domain
{
	public class XsReceReceivableDTO : BaseDTO<string>
	{
		#region 实体成员
		/// <summary>
		/// 
		/// </summary>
		public string XRC_ReceId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? XRC_SetDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_Origin { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_BillID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_Client { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_Area { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XRC_Total { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XRC_Earning { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? XRC_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_CementType { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string XRC_Remark { set; get; }
		#endregion

		#region 扩展字段
		/// <summary>
		/// ID
		/// </summary>
		public List<string> XRC_Origins { set; get; } = new List<string>();
		#endregion

		#region 扩展操作
		/// <summary>
		/// 搜索表达式
		/// </summary>
		/// <returns></returns>
		public Expression<Func<XsReceReceivableDO, bool>> GetExpression()
		{
			Expression<Func<XsReceReceivableDO, bool>> expression = oo => oo.IsDel == 0;

			if (!string.IsNullOrEmpty(XRC_BillID))
				expression = expression.And(oo => oo.XRC_BillID == XRC_BillID);
			if (XRC_Origins != null && XRC_Origins.Count > 0)
			{
				if (XRC_Origins.Count == 1)
				{
					var XRC_Origin = XRC_Origins[0];
					expression = expression.And(oo => oo.XRC_Origin == XRC_Origin);
					return expression;
				}
				else
				{
					expression = expression.And(oo => XRC_Origins.Contains(oo.XRC_Origin));
					return expression;
				}
			}

			return expression;
		}
		#endregion
	}
}
