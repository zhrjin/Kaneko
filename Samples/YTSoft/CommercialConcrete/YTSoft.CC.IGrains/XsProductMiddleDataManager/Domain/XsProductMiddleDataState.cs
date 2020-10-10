using AutoMapper;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain
{
	[AutoMap(typeof(XsProductMiddleDataDO))]
	public class XsProductMiddleDataState : BaseState<string>
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
	}
}