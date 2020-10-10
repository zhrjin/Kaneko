using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsTaskBaseManager.Domain
{
    [AutoMap(typeof(XsTaskBaseDTO))]
    [AutoMap(typeof(XsTaskBaseState))]
    [KanekoTable(name: "XS_Task_Base", isAutoUpdate: true)]
    public class XsTaskBaseDO : SqlServerWithOperatorBaseDO<string>, IViewObject
    {
        [KanekoId]
        [KanekoColumn(Name = "id", ColumnDefinition = "varchar(36) not null primary key")]
        public override string Id { get; set; }

        [KanekoColumn(Name = "XTB_Code", ColumnDefinition = "varchar(36) null")]
        public string XTB_Code { set; get; }

        [KanekoColumn(Name = "XTB_SetDate", ColumnDefinition = "datetime null")]
        public DateTime? XTB_SetDate { set; get; }

        [KanekoColumn(Name = "XTB_Area", ColumnDefinition = "varchar(36) null")]
        public string XTB_Area { get; set; }

        [KanekoColumn(Name = "XTB_Contract", ColumnDefinition = "varchar(36) null")]
        public string XTB_Contract { get; set; }

        [KanekoColumn(Name = "XTB_InDent", ColumnDefinition = "varchar(36) null")]
        public string XTB_InDent { get; set; }

        [KanekoColumn(Name = "XTB_TaskType", ColumnDefinition = "varchar(36) null")]
        public string XTB_TaskType { get; set; }

        [KanekoColumn(Name = "XTB_Client", ColumnDefinition = "varchar(36) null")]
        public string XTB_Client { get; set; }

        [KanekoColumn(Name = "XTB_ProName", ColumnDefinition = "varchar(100) null")]
        public string XTB_ProName { get; set; }

        [KanekoColumn(Name = "XTB_ArName", ColumnDefinition = "varchar(50) null")]
        public string XTB_ArName { get; set; }

        [KanekoColumn(Name = "XTB_ArSpace", ColumnDefinition = "varchar(50) null")]
        public string XTB_ArSpace { get; set; }

        [KanekoColumn(Name = "XTB_Location", ColumnDefinition = "varchar(50) null")]
        public string XTB_Location { get; set; }

        [KanekoColumn(Name = "XTB_Cement", ColumnDefinition = "varchar(36) null")]
        public string XTB_Cement { get; set; }

        [KanekoColumn(Name = "XTB_Auxiliary", ColumnDefinition = "varchar(50) null")]
        public string XTB_Auxiliary { get; set; }

        [KanekoColumn(Name = "XTB_Pumper", ColumnDefinition = "varchar(36) null")]
        public string XTB_Pumper { get; set; }

        [KanekoColumn(Name = "XTB_Salesman", ColumnDefinition = "varchar(36) null")]
        public string XTB_Salesman { get; set; }

        [KanekoColumn(Name = "XTB_Number", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_Number { get; set; }

        [KanekoColumn(Name = "XTB_Price", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_Price { get; set; }

        [KanekoColumn(Name = "XTB_TotalPrice", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_TotalPrice { get; set; }

        [KanekoColumn(Name = "XTB_Total", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_Total { get; set; }

        [KanekoColumn(Name = "XTB_BegDate", ColumnDefinition = "datetime null")]
        public DateTime? XTB_BegDate { get; set; }

        [KanekoColumn(Name = "XTB_EndDate", ColumnDefinition ="datetime null")]
        public DateTime? XTB_EndDate { get; set; }

        [KanekoColumn(Name = "XTB_Casting", ColumnDefinition = "varchar(100) null")]
        public string XTB_Casting { get; set; }

        [KanekoColumn(Name = "XTB_Slumps", ColumnDefinition = "varchar(50) null")]
        public string XTB_Slumps { get; set; }

        [KanekoColumn(Name = "XTB_Maximum", ColumnDefinition = "varchar(36) null")]
        public string XTB_Maximum { get; set; }

        [KanekoColumn(Name = "XTB_LinkMan", ColumnDefinition = "varchar(36) null")]
        public string XTB_LinkMan { get; set; }

        [KanekoColumn(Name = "XTB_LinkTel", ColumnDefinition = "varchar(36) null")]
        public string XTB_LinkTel { get; set; }

        [KanekoColumn(Name = "XTB_SendState", ColumnDefinition = "char(1) null")]
        public string XTB_SendState { get; set; }

        [KanekoColumn(Name = "XTB_Remark", ColumnDefinition = "varchar(200) null")]
        public string XTB_Remark { get; set; }

        [KanekoColumn(Name = "XTB_AuditFlow", ColumnDefinition = "varchar(36) null")]
        public string XTB_AuditFlow { get; set; }

        [KanekoColumn(Name = "XTB_AuditTache", ColumnDefinition = "varchar(36) null")]
        public string XTB_AuditTache { get; set; }

        [KanekoColumn(Name = "XTB_AuditState", ColumnDefinition = "varchar(3) null")]
        public string XTB_AuditState { get; set; }

        [KanekoColumn(Name = "XTB_IsStop", ColumnDefinition = "char(1) null")]
        public string XTB_IsStop { get; set; }

        [KanekoColumn(Name = "XTB_PrnTimes", ColumnDefinition = "int null")]
        public int? XTB_PrnTimes { get; set; }

        [KanekoColumn(Name = "XTB_Status", ColumnDefinition = "varchar(36) null")]
        public string XTB_Status { get; set; }

        [KanekoColumn(Name = "XTB_Firm", ColumnDefinition = "varchar(36) null")]
        public string XTB_Firm { get; set; }

        [KanekoColumn(Name = "XTB_ProId", ColumnDefinition = "varchar(36) null")]
        public string XTB_ProId { get; set; }

        [KanekoColumn(Name = "XTB_Method", ColumnDefinition = "varchar(50) null")]
        public string XTB_Method { get; set; }

        [KanekoColumn(Name = "XTB_TaskName", ColumnDefinition = "varchar(100) null")]
        public string XTB_TaskName { get; set; }

        [KanekoColumn(Name = "XTB_Machine", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_Machine { get; set; }

        [KanekoColumn(Name = "XTB_PumperPrice", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_PumperPrice { get; set; }

        [KanekoColumn(Name = "XTB_AllPumper", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_AllPumper { get; set; }

        [KanekoColumn(Name = "XTB_FrePrice", ColumnDefinition = "numeric(18,6) null")]
        public decimal? XTB_FrePrice { get; set; }

        [KanekoColumn(Name = "XTB_IsSend", ColumnDefinition = "varchar(1) null")]
        public string XTB_IsSend { get; set; }

        [KanekoColumn(Name = "XTB_SendUser", ColumnDefinition = "varchar(36) null")]
        public string XTB_SendUser { get; set; }

        [KanekoColumn(Name = "XTB_SendDate", ColumnDefinition = "datetime null")]
        public DateTime? XTB_SendDate { get; set; }

        [KanekoColumn(Name = "XTB_IsCancel", ColumnDefinition = "varchar(1) null")]
        public string XTB_IsCancel { get; set; }

        [KanekoColumn(Name = "XTB_CancelUser", ColumnDefinition = "varchar(36) null")]
        public string XTB_CancelUser { get; set; }

        [KanekoColumn(Name = "XTB_CancelDate", ColumnDefinition = "datetime null")]
        public DateTime? XTB_CancelDate { get; set; }

        [KanekoColumn(Name = "XTB_Matching", ColumnDefinition = "varchar(36) null")]
        public string XTB_Matching { get; set; }

        [KanekoColumn(Name = "XTB_IsSupply", ColumnDefinition = "varchar(1) null")]
        public string XTB_IsSupply { get; set; }

        [KanekoColumn(Name = "XTB_Formula", ColumnDefinition = "varchar(36) null")]
        public string XTB_Formula { get; set; }







    }
}
