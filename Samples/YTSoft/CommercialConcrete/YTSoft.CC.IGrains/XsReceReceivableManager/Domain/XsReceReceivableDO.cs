using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsReceReceivableManager.Domain
{
	[AutoMap(typeof(XsReceReceivableDTO))]
	[AutoMap(typeof(XsReceReceivableState))]
	[KanekoTable(name: "Xs_Rece_Receivable", isAutoUpdate: true)]
	public class XsReceReceivableDO : SqlServerWithOperatorBaseDO<string>,IViewObject
	{
		#region 实体成员
		/// <summary>
		/// 主键
		/// </summary>
		[KanekoId]
		[KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
		public override string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_ReceId", ColumnDefinition = "varchar(36) null")]
		public string XRC_ReceId { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_SetDate", ColumnDefinition = "datetime null")]
		public DateTime? XRC_SetDate { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Origin", ColumnDefinition = "varchar(3) null")]
		public string XRC_Origin { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_BillID", ColumnDefinition = "varchar(36) null")]
		public string XRC_BillID { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Client", ColumnDefinition = "varchar(36) null")]
		public string XRC_Client { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Area", ColumnDefinition = "varchar(36) null")]
		public string XRC_Area { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Total", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XRC_Total { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Earning", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XRC_Earning { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Number", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XRC_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_CementType", ColumnDefinition = "varchar(36) null")]
		public string XRC_CementType { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Firm", ColumnDefinition = "varchar(36) null")]
		public string XRC_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XRC_Remark", ColumnDefinition = "varchar(2000) null")]
		public string XRC_Remark { set; get; }
		#endregion
	}
}
