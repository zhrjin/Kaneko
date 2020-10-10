using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    public class TableInfoDTO : IValidatableObject
    {
        /// <summary>
        /// 系统ID
        /// </summary>
        public long SystemId { set; get; }

        /// <summary>
        /// 组别
        /// </summary>
        public string GroupCode { set; get; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { set; get; }

        public List<ColumnInfoDTO> Columns { set; get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SystemId <= 0)
                yield return new ValidationResult("系统ID不能为空！", new[] { nameof(SystemId) });

            if (string.IsNullOrWhiteSpace(GroupCode))
                yield return new ValidationResult("组别不能为空！", new[] { nameof(GroupCode) });

            if (string.IsNullOrWhiteSpace(TableName))
                yield return new ValidationResult("表名不能为空！", new[] { nameof(TableName) });

            if (Columns == null || Columns.Count == 0)
                yield return new ValidationResult("表字段信息不能为空！", new[] { "Columns" });

            foreach (var column in Columns)
            {
                if (string.IsNullOrWhiteSpace(column.SelfColumn))
                    yield return new ValidationResult("列名不能为空！", new[] { nameof(column.SelfColumn) });

                if (string.IsNullOrWhiteSpace(column.ModelColumn))
                    yield return new ValidationResult("列对应模型名不能为空！", new[] { nameof(column.ModelColumn) });
            }
        }
    }
}
