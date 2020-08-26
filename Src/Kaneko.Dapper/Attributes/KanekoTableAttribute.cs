using System;
namespace Kaneko.Dapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public  class KanekoTableAttribute:Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        public KanekoTableAttribute(string name)
        {
            Name = name;
        }
    }
}
