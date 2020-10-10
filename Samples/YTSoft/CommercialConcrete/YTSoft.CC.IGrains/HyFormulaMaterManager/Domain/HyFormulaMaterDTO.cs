using Kaneko.Core.Contract;
using System;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.HyFormulaMaterManager.Domain
{
	public class HyFormulaMaterDTO : BaseDTO<string>
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

		#region 扩展操作
		/// <summary>
		/// 搜索表达式
		/// </summary>
		/// <returns></returns>
		public Expression<Func<HyFormulaMaterDO, bool>> GetExpression()
		{
			Expression<Func<HyFormulaMaterDO, bool>> expression = oo => oo.IsDel == 0;

			return expression;
		}
		#endregion
	}
}
