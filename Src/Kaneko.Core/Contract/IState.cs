using System;

namespace Kaneko.Core.Contract
{
    public class BsseState : IState
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModityBy { set; get; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModityDate { set; get; }

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        public int Version { set; get; }
    }

    public interface IState
    {
    }
}
