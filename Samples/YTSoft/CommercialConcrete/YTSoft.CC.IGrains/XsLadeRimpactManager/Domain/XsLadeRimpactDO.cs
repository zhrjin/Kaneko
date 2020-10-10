using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager.Domain
{
	[AutoMap(typeof(XsLadeRimpactDTO))]
	[AutoMap(typeof(XsLadeRimpactState))]
	[KanekoTable(name: "Xs_Lade_Rimpact", isAutoUpdate: true)]
	public class XsLadeRimpactDO : SqlServerWithOperatorBaseDO<string>
	{
		#region 实体成员
		/// <summary>
		/// 主键
		/// </summary>
		[KanekoId]
		[KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
		public override string Id { get; set; }
		/// <summary>
		/// 发货单主键
		/// </summary>
		[KanekoColumn(Name = "XLR_Lade", ColumnDefinition = "varchar(36) null")]
		public string XLR_Lade { set; get; }
		/// <summary>
		/// 作废时间
		/// </summary>
		[KanekoColumn(Name = "XLR_SetDate", ColumnDefinition = "datetime null")]
		public DateTime? XLR_SetDate { set; get; }
		/// <summary>
		/// 作废类型
		/// </summary>
		[KanekoColumn(Name = "XLR_Type", ColumnDefinition = "varchar(3) null")]
		public string XLR_Type { set; get; }
		/// <summary>
		/// 作废数量
		/// </summary>
		[KanekoColumn(Name = "XLR_Number", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLR_Number { set; get; }
		/// <summary>
		/// 
		/// </summary>
		[KanekoColumn(Name = "XLR_Total", ColumnDefinition = "numeric(18,6) null")]
		public decimal? XLR_Total { set; get; }
		/// <summary>
		/// 所属企业主键
		/// </summary>
		[KanekoColumn(Name = "XLR_Firm", ColumnDefinition = "varchar(36) null")]
		public string XLR_Firm { set; get; }
		/// <summary>
		/// 备注
		/// </summary>
		[KanekoColumn(Name = "XLR_Remark", ColumnDefinition = "varchar(2000) null")]
		public string XLR_Remark { set; get; }
		#endregion
	}
}
