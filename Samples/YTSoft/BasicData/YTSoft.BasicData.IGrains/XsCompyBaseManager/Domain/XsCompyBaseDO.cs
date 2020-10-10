using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain
{
    /// <summary>
    /// isAutoUpdate：false 不自动更新表结果
    /// </summary>
    [KanekoTable(name: "XS_Compy_Base_Temp2", isAutoUpdate: false)] /* 测试先使用临时表 */
    public class XsCompyBaseDO : IDomainObject, IViewObject
    {
        #region 实体成员
        /// <summary>
        /// 客商主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "XOB_ID")]
        public string XOB_ID { set; get; }

        /// <summary>
        /// 客商名称
        /// </summary>
        [KanekoColumn(Name = "XOB_Name")]
        public string XOB_Name { set; get; }

        /// <summary>
        /// 对应企业
        /// </summary>
        [KanekoColumn(Name = "XOB_Firm")]
        public string XOB_Firm { set; get; }
        #endregion
    }
}
