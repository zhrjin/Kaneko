using Kaneko.Core.Contract;
using System.Collections.Generic;
using TYSoft.Common.Model.DataSync;

namespace TYSoft.Common.Model.ComConcrete
{
    public class DataSyncModel : IViewObject
    {
        public List<TableInfoModel> Tables { set; get; }
    }
}
