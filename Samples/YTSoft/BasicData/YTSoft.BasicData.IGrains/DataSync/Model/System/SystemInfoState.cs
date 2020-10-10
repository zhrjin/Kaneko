using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [AutoMap(typeof(SystemInfoDO))]
    [Serializable]
    public class SystemInfoState
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { set; get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnection { set; get; }

        /// <summary>
        /// 1-已删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string Firm { set; get; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { set; get; }
    }
}
