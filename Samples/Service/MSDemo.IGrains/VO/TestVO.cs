using AutoMapper;
using Kaneko.Core.Contract;
using MSDemo.IGrains.Entity;

namespace MSDemo.IGrains.VO
{
    [AutoMap(typeof(TestDO))]
    public class TestVO : BaseVO
    {
        public string UserId { set; get; }
    }
}
