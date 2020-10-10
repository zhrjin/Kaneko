using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.HyFormulaMaterManager.Domain
{
	[AutoMap(typeof(HyFormulaMaterDTO))]
	[AutoMap(typeof(HyFormulaMaterState))]
	[KanekoTable(name: "HY_Formula_Mater", isAutoUpdate: true)]
	public class HyFormulaMaterDO : SqlServerWithOperatorBaseDO<string>, IViewObject
	{
		#region 实体成员
		/// <summary>
		/// 主键
		/// </summary>
		[KanekoId]
		[KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
		public override string Id { get; set; }
		/// <summary>
		/// 物料名称
		/// </summary>
		[KanekoColumn(Name = "HFM_Name", ColumnDefinition = "varchar(36) null")]
		public string HFM_Name { set; get; }
		/// <summary>
		/// 云天物料主键
		/// </summary>
		[KanekoColumn(Name = "HFM_Cement", ColumnDefinition = "varchar(36) null")]
		public string HFM_Cement { set; get; }
		/// <summary>
		/// 生产线
		/// </summary>
		[KanekoColumn(Name = "HFM_Line", ColumnDefinition = "varchar(36) null")]
		public string HFM_Line { set; get; }
		/// <summary>
		/// 标定值字段
		/// </summary>
		[KanekoColumn(Name = "HFM_Field", ColumnDefinition = "varchar(36) null")]
		public string HFM_Field { set; get; }
		/// <summary>
		/// 实际消耗值字段
		/// </summary>
		[KanekoColumn(Name = "HFM_FactField", ColumnDefinition = "varchar(36) null")]
		public string HFM_FactField { set; get; }
		/// <summary>
		/// 企业主键
		/// </summary>
		[KanekoColumn(Name = "HFM_Firm", ColumnDefinition = "varchar(36) null")]
		public string HFM_Firm { set; get; }
		/// <summary>
		/// 排序
		/// </summary>
		[KanekoColumn(Name = "HFM_Order", ColumnDefinition = "int null")]
		public int? HFM_Order { set; get; }
		#endregion
	}
}
