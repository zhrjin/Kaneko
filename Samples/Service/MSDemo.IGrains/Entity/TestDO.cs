using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using MSDemo.IGrains.VO;

namespace MSDemo.IGrains.Entity
{
    [AutoMap(typeof(TestVO))]
    [KanekoTable(name: "t_test")]
    public class TestDO : BaseDO
    {
        [KanekoId]
        [KanekoColumn(Name = "user_id", ColumnDefinition = "varchar(20) null")]
        public string UserId { set; get; }

        [KanekoColumn(Name = "user_name", ColumnDefinition = "varchar(255) null")]
        public string UserName { set; get; }
    }
}
