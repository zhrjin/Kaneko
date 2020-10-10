using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    [Serializable]
    public class DataSyncGroupState : IState, IViewObject
    {
        public GrainDataState GrainDataState { get; set; }

        public List<TableInfoState> Tables { set; get; } = new List<TableInfoState>();
    }
}
