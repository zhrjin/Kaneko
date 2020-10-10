using AutoMapper;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager.Domain
{
	[AutoMap(typeof(XsLadeRimpactDO))]
	[AutoMap(typeof(XsLadeRimpactState))]
	public class XsLadeRimpactVO : BaseVO<string>
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
		/// 发货单编码
		/// </summary>
		public string XLB_LadeId { set; get; }
		/// <summary>
		/// 客商名称
		/// </summary>
		public string XLB_ClientName { set; get; }
		/// <summary>
		/// 所属企业名称
		/// </summary>
		public string XLR_FirmName { set; get; }
		#endregion
	}
}
