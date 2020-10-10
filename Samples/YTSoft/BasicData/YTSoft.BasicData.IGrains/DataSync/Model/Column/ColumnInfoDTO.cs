using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    public class ColumnInfoDTO : IValidatableObject
    {
        public long TableId { set; get; }

        /// <summary>
        /// 自身列名称
        /// </summary>
        public string SelfColumn { set; get; }

        /// <summary>
        /// 模型列名称
        /// </summary>
        public string ModelColumn { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { set; get; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int SortNo { set; get; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string DisplayName { set; get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TableId <= 0)
                yield return new ValidationResult("表ID不能为空！", new[] { nameof(TableId) });

            if (string.IsNullOrWhiteSpace(SelfColumn))
                yield return new ValidationResult("自身列名称不能为空！", new[] { nameof(SelfColumn) });

            if (string.IsNullOrWhiteSpace(ModelColumn))
                yield return new ValidationResult("模型列名称不能为空！", new[] { nameof(ModelColumn) });
        }
    }
}
