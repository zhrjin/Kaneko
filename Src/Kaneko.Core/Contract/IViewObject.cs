using System;

namespace Kaneko.Core.Contract
{
    /// <summary>
    /// 视图对象，用于展示层VO
    /// </summary>
    public interface IViewObject
    {
    }

    public abstract class BaseVO<PrimaryKey> : IViewObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        public PrimaryKey Id { set; get; }

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        public int Version { set; get; }

        /// <summary>
        /// 是否删除1-删除
        /// </summary>
        public int IsDel { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { set; get; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateByName { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModityBy { set; get; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModityByName { set; get; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModityDate { set; get; }
    }
}
