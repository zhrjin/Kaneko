using AutoMapper;
using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;
using System.Text;


namespace YTSoft.CC.IGrains.XsTaskDetailManager.Domain
{
    [AutoMap(typeof(XsTaskDetailDO))]
    [AutoMap(typeof(XsTaskDetailState))]
    public class XsTaskDetailVO: BaseVO<string>
    {
        public string XTD_TaskID { set; get; }

        public string XTD_Auxiliary { set; get; }

        public decimal? XTD_Number { get; set; }

        public decimal? XTD_Price { get; set; }

        public decimal? XTD_Total { get; set; }

        public int? XTD_Order { get; set; }

        public string XTD_Remark { get; set; }

        public string XTD_Firm { get; set; }
    }
}
