using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain
{
    /// <summary>
    /// isAutoUpdate：false 不自动更新表结果
    /// </summary>
    [KanekoTable(name: "PB_Code_Material_Temp", isAutoUpdate: false)] /* 测试先使用临时表 */
    public class PbCodeMaterialDO : IDomainObject, IViewObject
    {
        #region 实体成员
        /// <summary>
        /// 物料主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "PCM_ID")]
        public string PCM_ID { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [KanekoColumn(Name = "PCM_Name")]
        public string PCM_Name { set; get; }

        /// <summary>
        /// 对应企业
        /// </summary>
        [KanekoColumn(Name = "PCM_Firm")]
        public string PCM_Firm{  set; get; }
        #endregion
    }
}
