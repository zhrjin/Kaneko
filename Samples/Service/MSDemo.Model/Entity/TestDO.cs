using AutoMapper;
using Kaneko.Dapper.Attributes;
using Kaneko.Dapper.Contract;
using MSDemo.Model.VO;
namespace MSDemo.Model.Entity
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
