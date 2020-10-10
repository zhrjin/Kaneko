namespace TYSoft.Common.Model.DataSync
{
    public class SystemInfoModel
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

    }
}
