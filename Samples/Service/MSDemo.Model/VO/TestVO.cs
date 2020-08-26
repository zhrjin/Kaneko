using AutoMapper;
using Kaneko.Dapper.Contract;
using MSDemo.Model.Entity;

namespace MSDemo.Model.VO
{
    [AutoMap(typeof(TestDO))]
    public class TestVO : BaseVO
    {
        public string UserId { set; get; }
    }
}
