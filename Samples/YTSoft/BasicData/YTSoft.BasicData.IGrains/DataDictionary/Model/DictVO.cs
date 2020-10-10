using AutoMapper;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    [AutoMap(typeof(DictDO))]
    public class DictVO : BaseVO<long>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        public int Parentid { set; get; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string DataType { set; get; }

        /// <summary>
        /// 字典键
        /// </summary>
        public string DataCode { set; get; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string DataValue { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string DataDesc { set; get; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int SortNo { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string Firm { set; get; }
    }
}
