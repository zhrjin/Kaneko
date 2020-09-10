using System;
namespace Kaneko.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class KanekoIdAttribute : Attribute
    {
        /// <summary>
        /// 是否自增长字段
        /// </summary>
        public bool AutoIdEntity { set; get; } = false;
    }
}
