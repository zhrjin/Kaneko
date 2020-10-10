using AutoMapper;
using Kaneko.Core.Contract;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain
{
	[AutoMap(typeof(XsProductMiddleDataVO))]
	public class XsProductMiddleDataDTO : BaseDTO<string>
	{
		#region 实体成员
		public string XPM_EID { set; get; }

		public string XPM_Tagalong { set; get; }

		public string XPM_Matching { set; get; }

		public string XPM_CurDate { set; get; }

		public string XPM_JobNo { set; get; }

		public string XPM_PropNo { set; get; }

		public string XPM_Strength { set; get; }

		public string XPM_CustNm { set; get; }

		public string XPM_ProjNm { set; get; }

		public string XPM_Location { set; get; }

		public string XPM_SiteNo { set; get; }

		public string XPM_TechReq { set; get; }

		public string XPM_ETel { set; get; }

		public string XPM_DeliveryMode { set; get; }

		public string XPM_TTLQntyPlanned { set; get; }

		public string XPM_YSLC { set; get; }

		public string XPM_KangShenLevel { set; get; }

		public string XPM_PBBL { set; get; }

		public string XPM_SNBH { set; get; }

		public string XPM_SZGG { set; get; }

		public string XPM_TaLuoDu { set; get; }

		public string XPM_Remark { set; get; }

		public string XPM_LJFL { set; get; }

		public string XPM_FHBH { set; get; }

		public string XPM_Truckvol { set; get; }

		public string XPM_YSGJ { set; get; }

		public string XPM_CurTime { set; get; }

		public string XPM_JSY { set; get; }

		public string XPM_Operator { set; get; }

		public string XPM_FHR { set; get; }

		public string XPM_ProduceLine { set; get; }

		public string XPM_QYDD { set; get; }

		public string XPM_TruckNo { set; get; }

		public string XPM_Driver { set; get; }

		public string XPM_TBBJ { set; get; }

		public string XPM_SCBJ { set; get; }

		public string XPM_YLa { set; get; }

		public string XPM_YLb { set; get; }

		public string XPM_SCXH { set; get; }

		public string XPM_IsTransMit { set; get; }

		public string XPM_Line { set; get; }

		public string XPM_RepetBill { set; get; }
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
		public Expression<Func<XsProductMiddleDataDO, bool>> GetExpression()
		{
			Expression<Func<XsProductMiddleDataDO, bool>> expression = null;

			if (!string.IsNullOrEmpty(XPM_EID))
			{
				Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => oo.XPM_EID.Contains(XPM_EID);
				expression = expression == null ? exp : expression.And(exp);
			}
			if (!string.IsNullOrEmpty(XPM_TruckNo))
			{
				Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => oo.XPM_TruckNo.Contains(XPM_TruckNo);
				expression = expression == null ? exp : expression.And(exp);
			}
			if (Ids != null && Ids.Count > 0)
			{
				if (Ids.Count == 1)
				{
					var id = Ids[0];
					Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => oo.Id == id;
					expression = expression == null ? exp : expression.And(exp);
				}
				else
				{
					Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => Ids.Contains(oo.Id);
					expression = expression == null ? exp : expression.And(exp);
				}
			}
			if (!string.IsNullOrEmpty(StartTime))
			{
				DateTime dt = DateTime.Parse(StartTime);
				Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => Convert.ToDateTime(oo.XPM_CurDate) >= dt;
				expression = expression == null ? exp : expression.And(exp);
			}
			if (!string.IsNullOrEmpty(EndTime))
			{
				DateTime dt = DateTime.Parse(EndTime);
				Expression<Func<XsProductMiddleDataDO, bool>> exp = oo => Convert.ToDateTime(oo.XPM_CurDate) <= dt;
				expression = expression == null ? exp : expression.And(exp);
			}

			return expression;
		}
		/// <summary>
		/// 搜索表达式
		/// </summary>
		/// <returns></returns>
		public Expression<Func<XsProductMiddleDataDO, bool>> GetExpression2()
		{
			Expression<Func<XsProductMiddleDataDO, bool>> expression = oo => oo.IsDel == 0;
			expression = expression.And(oo => oo.XPM_EID == XPM_EID);
			//expression = expression.And(oo => oo.XLB_Firm == XLB_Firm);

			return expression;
		}
		#endregion
	}
}
