using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsLadeBaseManager.Domain
{
	[AutoMap(typeof(XsLadeBaseDTO))]
	[AutoMap(typeof(XsLadeBaseState))]
	[AutoMap(typeof(XsLadeBaseVO))]
	[KanekoTable(name: "XS_Lade_Base", isAutoUpdate: true)]
	public class XsLadeBaseDO : SqlServerWithOperatorBaseDO<string>, IViewObject
	{
		#region 实体成员
		[KanekoId]
		[KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
		public override string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_LadeId", ColumnDefinition = "varchar(36) null")]
		public string XLB_LadeId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_SetDate", ColumnDefinition = "datetime null")]
		public DateTime? XLB_SetDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Area", ColumnDefinition = "varchar(36) null")]
		public string XLB_Area { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Origin", ColumnDefinition = "varchar(3) null")]
		public string XLB_Origin { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Line", ColumnDefinition = "varchar(36) null")]
		public string XLB_Line { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Client", ColumnDefinition = "varchar(36) null")]
		public string XLB_Client { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Cement", ColumnDefinition = "varchar(36) null")]
		public string XLB_Cement { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Number", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Price", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_Price { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_CardPrice", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_CardPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Total", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_Total { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_FactNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_FactNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_TurnNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_TurnNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ReturnNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_ReturnNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_FactTotal", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_FactTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ScaleDifNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_ScaleDifNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_InvoNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_InvoNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_SendArea", ColumnDefinition = "varchar(50) null")]
		public string XLB_SendArea { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ApproveMan", ColumnDefinition = "varchar(36) null")]
		public string XLB_ApproveMan { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_CarCode", ColumnDefinition = "varchar(50) null")]
		public string XLB_CarCode { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Quantity", ColumnDefinition = "int null")]
		public int? XLB_Quantity { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_PickMan", ColumnDefinition = "varchar(50) null")]
		public string XLB_PickMan { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_PrintNum", ColumnDefinition = "int null")]
		public int? XLB_PrintNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ScaleDifID", ColumnDefinition = "varchar(36) null")]
		public string XLB_ScaleDifID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_IsOut", ColumnDefinition = "char(1) null")]
		public string XLB_IsOut { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Gather", ColumnDefinition = "char(1) null")]
		public string XLB_Gather { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_IsInvo", ColumnDefinition = "char(1) null")]
		public string XLB_IsInvo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Collate", ColumnDefinition = "char(1) null")]
		public string XLB_Collate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Firm", ColumnDefinition = "varchar(36) null")]
		public string XLB_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Status", ColumnDefinition = "char(1) null")]
		public string XLB_Status { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Remark", ColumnDefinition = "varchar(2000) null")]
		public string XLB_Remark { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ReturnRemark", ColumnDefinition = "varchar(2000) null")]
		public string XLB_ReturnRemark { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_IsAgainPrint", ColumnDefinition = "char(1) null")]
		public string XLB_IsAgainPrint { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Tranist", ColumnDefinition = "varchar(36) null")]
		public string XLB_Tranist { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ColType", ColumnDefinition = "varchar(20) null")]
		public string XLB_ColType { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_AuditCarryTime", ColumnDefinition = "datetime null")]
		public DateTime? XLB_AuditCarryTime { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_TaskID", ColumnDefinition = "varchar(20) null")]
		public string XLB_TaskID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_BackNum", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_BackNum { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_IsTransmit", ColumnDefinition = "char(1) null")]
		public string XLB_IsTransmit { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_OptName", ColumnDefinition = "varchar(10) null")]
		public string XLB_OptName { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_OptID", ColumnDefinition = "varchar(20) null")]
		public string XLB_OptID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_CarNo", ColumnDefinition = "varchar(20) null")]
		public string XLB_CarNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_AgiId", ColumnDefinition = "varchar(20) null")]
		public string XLB_AgiId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Mortar", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_Mortar { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_Method", ColumnDefinition = "varchar(50) null")]
		public string XLB_Method { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ReturnPrice", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_ReturnPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_ReturnTotal", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_ReturnTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_AgentPrice", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_AgentPrice { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_AgentTotal", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLB_AgentTotal { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_RecipeNo", ColumnDefinition = "varchar(20) null")]
		public string XLB_RecipeNo { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLB_TurnType", ColumnDefinition = "varchar(20) null")]
		public string XLB_TurnType { set; get; }
		#endregion
	}
}
