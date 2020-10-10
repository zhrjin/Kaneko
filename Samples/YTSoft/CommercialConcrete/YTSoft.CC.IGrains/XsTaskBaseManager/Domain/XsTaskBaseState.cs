using AutoMapper;
using Kaneko.Core.Contract;
using System;

namespace YTSoft.CC.IGrains.XsTaskBaseManager.Domain
{
    [AutoMap(typeof(XsTaskBaseDO))]
    public class XsTaskBaseState : BaseState<string>
    {
        public string XTB_Code { set; get; }

        public DateTime? XTB_SetDate { set; get; }

        public string XTB_Area { get; set; }

        public string XTB_Contract { get; set; }

        public string XTB_InDent { get; set; }

        public string XTB_TaskType { get; set; }

        public string XTB_Client { get; set; }

        public string XTB_ProName { get; set; }

        public string XTB_ArName { get; set; }

        public string XTB_ArSpace { get; set; }

        public string XTB_Location { get; set; }

        public string XTB_Cement { get; set; }

        public string XTB_Auxiliary { get; set; }

        public string XTB_Pumper { get; set; }

        public string XTB_Salesman { get; set; }

        public decimal? XTB_Number { get; set; }

        public decimal? XTB_Price { get; set; }

        public decimal? XTB_TotalPrice { get; set; }

        public decimal? XTB_Total { get; set; }

        public DateTime? XTB_BegDate { get; set; }

        public DateTime? XTB_EndDate { get; set; }

        public string XTB_Casting { get; set; }

        public string XTB_Slumps { get; set; }

        public string XTB_Maximum { get; set; }

        public string XTB_LinkMan { get; set; }

        public string XTB_LinkTel { get; set; }

        public string XTB_SendState { get; set; }

        public string XTB_Remark { get; set; }

        public string XTB_AuditFlow { get; set; }

        public string XTB_AuditTache { get; set; }

        public string XTB_AuditState { get; set; }

        public string XTB_IsStop { get; set; }

        public int? XTB_PrnTimes { get; set; }

        public string XTB_Status { get; set; }

        public string XTB_Firm { get; set; }

        public string XTB_ProId { get; set; }

        public string XTB_Method { get; set; }

        public string XTB_TaskName { get; set; }

        public decimal? XTB_Machine { get; set; }

        public decimal? XTB_PumperPrice { get; set; }

        public decimal? XTB_AllPumper { get; set; }

        public decimal? XTB_FrePrice { get; set; }

        public string XTB_IsSend { get; set; }

        public string XTB_SendUser { get; set; }

        public DateTime? XTB_SendDate { get; set; }

        public string XTB_IsCancel { get; set; }

        public string XTB_CancelUser { get; set; }

        public DateTime? XTB_CancelDate { get; set; }

        public string XTB_Matching { get; set; }

        public string XTB_IsSupply { get; set; }

        public string XTB_Formula { get; set; }
    }
}
