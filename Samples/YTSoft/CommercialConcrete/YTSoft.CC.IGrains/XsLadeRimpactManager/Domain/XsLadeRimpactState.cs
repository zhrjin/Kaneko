using AutoMapper;
using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;

namespace YTSoft.CC.IGrains.XsLadeRimpactManager.Domain
{
	[AutoMap(typeof(XsLadeRimpactDO))]
	public class XsLadeRimpactState : BaseState<string>
	{
		public Dictionary<Guid, bool> Transactions { get; set; } = new Dictionary<Guid, bool>();

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
	}
}
