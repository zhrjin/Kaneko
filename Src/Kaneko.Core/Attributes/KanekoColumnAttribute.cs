using System;
namespace Kaneko.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class KanekoColumnAttribute : Attribute
    {
        public KanekoColumnAttribute()
        {
        }

        public KanekoColumnAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 唯一约束（默认为非）
        /// </summary>
        public bool Unique { get; set; } = false;

        /// <summary>
        /// 可空（默认为空）
        /// </summary>
        public bool Nullable { get; set; } = true;

        /// <summary>
        /// 列定义
        /// </summary>
        public string ColumnDefinition { get; set; }
    }
}
