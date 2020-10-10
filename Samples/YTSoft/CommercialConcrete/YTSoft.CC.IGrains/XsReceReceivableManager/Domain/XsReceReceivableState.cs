using AutoMapper;
using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;

namespace YTSoft.CC.IGrains.XsReceReceivableManager.Domain
{
	[AutoMap(typeof(XsReceReceivableDO))]
	public class XsReceReceivableState : BaseState<string>
	{
		public Dictionary<Guid, bool> Transactions { get; set; } = new Dictionary<Guid, bool>();

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
	}
}
