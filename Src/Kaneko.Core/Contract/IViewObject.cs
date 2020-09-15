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
    }
}
