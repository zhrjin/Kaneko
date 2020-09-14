using System;
namespace Kaneko.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class KanekoTableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表结构自动更新，默认true
        /// </summary>
        public bool IsAutoUpdate { get; set; }

        public KanekoTableAttribute(string name)
        {
            Name = name;
            IsAutoUpdate = true;
        }

        public KanekoTableAttribute(string name, bool isAutoUpdate)
        {
            Name = name;
            IsAutoUpdate = isAutoUpdate;
        }
    }
}
