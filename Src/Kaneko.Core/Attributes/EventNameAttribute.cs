using System;
namespace Kaneko.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventNameAttribute : Attribute
    {
        public EventNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { set; get; }
    }
}
