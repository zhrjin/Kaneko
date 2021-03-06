﻿using System;

namespace Kaneko.Core.Contract
{
    //[Serializable]
    public class BaseState<PrimaryKey> : IState
    {
        /// <summary>
        /// 主键
        /// </summary>
        public PrimaryKey Id { set; get; }

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

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        public int Version { set; get; }

        /// <summary>
        /// 是否删除 1-已删除
        /// </summary>
        public int IsDel { set; get; }

        public GrainDataState GrainDataState { set; get; }
    }

    public class BaseState<PrimaryKey, TStateClass> : BaseState<PrimaryKey>, ICloneable<TStateClass>
    {
        public virtual TStateClass Clone()
        {
            return (TStateClass)MemberwiseClone();
        }
    }

    public interface IState
    {
        GrainDataState GrainDataState { set; get; }
    }

    public enum GrainDataState
    {
        Init = 0,
        Loaded = 1
    }
}
