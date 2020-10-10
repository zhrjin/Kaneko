using Kaneko.Core.Attributes;
using System;
using TYSoft.Common.Model.EventBus;

namespace TYSoft.Common.Model.Test
{
    [EventName(EventContract.Tester.Test)]
    public class TestModel
    {
        public string Id { set; get; }

        public DateTime CreateDate { set; get; }

        public string Attribute01 { set; get; }
        public string Attribute02 { set; get; }
        public string Attribute03 { set; get; }
        public string Attribute04 { set; get; }
        public string Attribute05 { set; get; }
        public string Attribute06 { set; get; }
        public string Attribute07 { set; get; }
        public string Attribute08 { set; get; }
        public string Attribute09 { set; get; }
        public string Attribute10 { set; get; }
        public string Attribute11 { set; get; }
        public string Attribute12 { set; get; }
        public string Attribute13 { set; get; }
    }
}
