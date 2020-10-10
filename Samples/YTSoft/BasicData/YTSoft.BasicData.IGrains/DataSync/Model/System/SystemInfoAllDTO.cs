using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    public class SystemInfoAllDTO : IValidatableObject
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { set; get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnection { set; get; }

        /// <summary>
        /// 系统备注
        /// </summary>
        public string Desc { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string Firm { set; get; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { set; get; }

        public List<TableInfoDTO> Tables { set; get; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(DbConnection))
                yield return new ValidationResult("数据库连接字符串不能为空！", new[] { nameof(DbConnection) });

            if (Tables == null || Tables.Count == 0)
                yield return new ValidationResult("表信息不能为空！", new[] { nameof(Tables) });

            foreach (var table in Tables)
            {
                if (string.IsNullOrWhiteSpace(table.GroupCode))
                    yield return new ValidationResult("组别不能为空！", new[] { nameof(table.GroupCode) });

                if (string.IsNullOrWhiteSpace(table.TableName))
                    yield return new ValidationResult("表名不能为空！", new[] { nameof(table.TableName) });

                if (table.Columns == null || table.Columns.Count == 0)
                    yield return new ValidationResult("表字段信息不能为空！", new[] { "Columns" });

                foreach (var column in table.Columns)
                {
                    if (string.IsNullOrWhiteSpace(column.SelfColumn))
                        yield return new ValidationResult("列名不能为空！", new[] { nameof(column.SelfColumn) });

                    if (string.IsNullOrWhiteSpace(column.ModelColumn))
                        yield return new ValidationResult("列对应模型名不能为空！", new[] { nameof(column.ModelColumn) });
                }
            }
        }
    }
}
