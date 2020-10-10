using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using Orleans;
using System;
using YTSoft.CC.IGrains.XsTaskDetailManager;
using YTSoft.CC.Grains.XsTaskDetailManager.Repository;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;

namespace YTSoft.CC.Grains.XsTaskDetailManager
{
    [Reentrant]
    public class XsTaskDetailGrain : MainGrain, IXsTaskDetailGrain
    {
        private readonly IXsTaskDetailRepository _XsTaskDetailRepository;
        private readonly IClusterClient _clusterClient;

        public XsTaskDetailGrain(IXsTaskDetailRepository XsTaskDetailRepository, IClusterClient clusterClient)
        {
            this._XsTaskDetailRepository = XsTaskDetailRepository;
            this._clusterClient = clusterClient;
        }


        /// <summary>
        /// 异常处理
        /// </summary>
        protected override Func<Exception, Task> FuncExceptionHandler => (exception) =>
        {
            return Task.CompletedTask;
        };
    }
}
