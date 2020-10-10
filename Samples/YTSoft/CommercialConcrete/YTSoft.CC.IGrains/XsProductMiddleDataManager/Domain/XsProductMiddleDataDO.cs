using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain
{
	[AutoMap(typeof(XsProductMiddleDataDTO))]
	[AutoMap(typeof(XsProductMiddleDataState))]
	[AutoMap(typeof(XsProductMiddleDataVO))]
	[KanekoTable(name: "XS_Product_MiddleData", isAutoUpdate: true)]
	public class XsProductMiddleDataDO : SqlServerWithOperatorBaseDO<string>, IViewObject
	{
		#region 实体成员
		[KanekoId]
		[KanekoColumn(Name = "ID", ColumnDefinition = "varchar(50) not null primary key")]
		public override string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_EID", ColumnDefinition = "varchar(50) not null")]
		public string XPM_EID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Tagalong", ColumnDefinition = "varchar(200) null")]
		public string XPM_Tagalong { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Matching", ColumnDefinition = "varchar(200) null")]
		public string XPM_Matching { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_CurDate", ColumnDefinition = "varchar(200) null")]
		public string XPM_CurDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_JobNo", ColumnDefinition = "varchar(200) null")]
		public string XPM_JobNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_PropNo", ColumnDefinition = "varchar(200) null")]
		public string XPM_PropNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Strength", ColumnDefinition = "varchar(200) null")]
		public string XPM_Strength { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_CustNm", ColumnDefinition = "varchar(200) null")]
		public string XPM_CustNm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_ProjNm", ColumnDefinition = "varchar(200) null")]
		public string XPM_ProjNm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Location", ColumnDefinition = "varchar(200) null")]
		public string XPM_Location { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_SiteNo", ColumnDefinition = "varchar(200) null")]
		public string XPM_SiteNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_TechReq", ColumnDefinition = "varchar(200) null")]
		public string XPM_TechReq { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_ETel", ColumnDefinition = "varchar(200) null")]
		public string XPM_ETel { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_DeliveryMode", ColumnDefinition = "varchar(200) null")]
		public string XPM_DeliveryMode { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_TTLQntyPlanned", ColumnDefinition = "varchar(200) null")]
		public string XPM_TTLQntyPlanned { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_YSLC", ColumnDefinition = "varchar(200) null")]
		public string XPM_YSLC { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_KangShenLevel", ColumnDefinition = "varchar(200) null")]
		public string XPM_KangShenLevel { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_PBBL", ColumnDefinition = "varchar(200) null")]
		public string XPM_PBBL { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_SNBH", ColumnDefinition = "varchar(200) null")]
		public string XPM_SNBH { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_SZGG", ColumnDefinition = "varchar(200) null")]
		public string XPM_SZGG { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_TaLuoDu", ColumnDefinition = "varchar(200) null")]
		public string XPM_TaLuoDu { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Remark", ColumnDefinition = "varchar(200) null")]
		public string XPM_Remark { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_LJFL", ColumnDefinition = "varchar(200) null")]
		public string XPM_LJFL { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_FHBH", ColumnDefinition = "varchar(200) null")]
		public string XPM_FHBH { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Truckvol", ColumnDefinition = "varchar(200) null")]
		public string XPM_Truckvol { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_YSGJ", ColumnDefinition = "varchar(200) null")]
		public string XPM_YSGJ { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_CurTime", ColumnDefinition = "varchar(200) null")]
		public string XPM_CurTime { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_JSY", ColumnDefinition = "varchar(200) null")]
		public string XPM_JSY { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Operator", ColumnDefinition = "varchar(200) null")]
		public string XPM_Operator { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_FHR", ColumnDefinition = "varchar(200) null")]
		public string XPM_FHR { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_ProduceLine", ColumnDefinition = "varchar(200) null")]
		public string XPM_ProduceLine { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_QYDD", ColumnDefinition = "varchar(200) null")]
		public string XPM_QYDD { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_TruckNo", ColumnDefinition = "varchar(200) null")]
		public string XPM_TruckNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Driver", ColumnDefinition = "varchar(200) null")]
		public string XPM_Driver { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_TBBJ", ColumnDefinition = "varchar(200) null")]
		public string XPM_TBBJ { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_SCBJ", ColumnDefinition = "varchar(200) null")]
		public string XPM_SCBJ { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_YLa", ColumnDefinition = "varchar(200) null")]
		public string XPM_YLa { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_YLb", ColumnDefinition = "varchar(200) null")]
		public string XPM_YLb { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_SCXH", ColumnDefinition = "varchar(200) null")]
		public string XPM_SCXH { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_IsTransMit", ColumnDefinition = "varchar(200) null")]
		public string XPM_IsTransMit { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_Line", ColumnDefinition = "varchar(200) null")]
		public string XPM_Line { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XPM_RepetBill", ColumnDefinition = "varchar(200) null")]
		public string XPM_RepetBill { set; get; }
		#endregion
	}
}
