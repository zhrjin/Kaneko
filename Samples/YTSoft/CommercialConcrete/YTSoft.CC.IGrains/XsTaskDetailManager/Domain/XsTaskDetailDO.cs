using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsTaskDetailManager.Domain
{
    [AutoMap(typeof(XsTaskDetailDTO))]
    [KanekoTable(name: "XS_Task_Detail")]
    public class XsTaskDetailDO : SqlServerBaseDO<string>
    {
        [KanekoId]
        [KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
        public override string Id { get; set; }

        [KanekoColumn(Name = "XTD_TaskID",ColumnDefinition ="varchar(36) null")]
        public string XTD_TaskID { set; get; }

        [KanekoColumn(Name = "XTD_Auxiliary", ColumnDefinition = "varchar(36) null")]
        public string XTD_Auxiliary { set; get; }

        [KanekoColumn(Name = "XTD_Number", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTD_Number { get; set; }

        [KanekoColumn(Name = "XTD_Price", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTD_Price { get; set; }

        [KanekoColumn(Name = "XTD_Total", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTD_Total { get; set; }

        [KanekoColumn(Name="XTD_Order",ColumnDefinition ="int null")]
        public int? XTD_Order { get; set; }

        [KanekoColumn(Name = "XTD_Remark", ColumnDefinition = "varchar(36) null")]
        public string XTD_Remark { get; set; }

        [KanekoColumn(Name = "XTD_Firm", ColumnDefinition = "varchar(36) null")]
        public string XTD_Firm { get; set; }







    }
}
