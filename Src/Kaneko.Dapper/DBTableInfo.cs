using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Dapper
{
   public class DBTableInfo
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 是否可为null
        /// </summary>
        public string Nullable { set; get; }

        /// <summary>
        /// 列类别
        /// </summary>
        public string DataType { set; get; }
        
        /// <summary>
        /// 列长度
        /// </summary>
        public int Size { set; get; }
        
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public string PrimaryKey { set; get; }
    }
}
