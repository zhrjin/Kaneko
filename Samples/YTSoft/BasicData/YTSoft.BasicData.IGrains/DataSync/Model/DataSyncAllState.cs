using Kaneko.Core.Contract;
using System.Collections.Generic;
using YTSoft.BasicData.IGrains.DataDictionary.Model;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    public class DataSyncAllState : IState, IViewObject
    {

        public List<SystemInfoDO> SystemInfos { set; get; } = new List<SystemInfoDO>();
        public List<TableInfoDO> TableInfos { set; get; } = new List<TableInfoDO>();
        public List<ColumnInfoDO> ColumnInfos { set; get; } = new List<ColumnInfoDO>();

        public GrainDataState GrainDataState { get; set; }
    }
}
