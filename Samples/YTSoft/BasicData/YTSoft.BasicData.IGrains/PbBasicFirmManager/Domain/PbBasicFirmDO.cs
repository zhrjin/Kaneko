using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain
{
    /// <summary>
    /// isAutoUpdate：false 不自动更新表结果
    /// </summary>
    [KanekoTable(name: "PB_Basic_Firm_Temp", isAutoUpdate: false)] /* 测试先使用临时表 */
    public class PbBasicFirmDO : IDomainObject, IViewObject
    {
        #region 实体成员
        /// <summary>
        /// 企业主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "PBF_ID")]
        public string PBF_ID { set; get; }

        /// <summary>
        /// 企业简称
        /// </summary>
        [KanekoColumn(Name = "PBF_ShortName")]
        public string PBF_ShortName { set; get; }
        #endregion
    }
}
