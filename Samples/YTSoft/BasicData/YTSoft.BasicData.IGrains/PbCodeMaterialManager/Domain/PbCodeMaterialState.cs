using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;

namespace YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain
{
    public class PbCodeMaterialState : IState
    {
        public GrainDataState GrainDataState { get; set; }

        /// <summary>
        /// 失效时间点，空-不失效
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        public List<PbCodeMaterialDO> Balance { get; set; } = new List<PbCodeMaterialDO>();
    }
}
