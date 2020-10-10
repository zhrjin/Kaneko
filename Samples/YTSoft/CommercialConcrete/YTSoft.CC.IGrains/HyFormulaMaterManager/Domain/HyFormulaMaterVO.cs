using AutoMapper;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.HyFormulaMaterManager.Domain
{
	[AutoMap(typeof(HyFormulaMaterDO))]
	[AutoMap(typeof(HyFormulaMaterState))]
	public class HyFormulaMaterVO : BaseVO<string>
	{
		#region 实体成员
		/// <summary>
		/// 
		/// </summary>
		public string HFM_Name { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string HFM_Cement { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string HFM_Line { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string HFM_Field { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string HFM_FactField { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public string HFM_Firm { set; get; }
		/// <summary>
		/// 
		/// </summary>
		public int? HFM_Order { set; get; }
		#endregion
	}
}
