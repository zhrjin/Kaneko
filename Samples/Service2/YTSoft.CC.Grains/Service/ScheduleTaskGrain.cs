using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTSoft.CC.Grains.Repository;
using YTSoft.CC.IGrains.Entity;
using YTSoft.CC.IGrains.Service;
using YTSoft.CC.IGrains.VO;
using Orleans.Concurrency;

namespace YTSoft.CC.Grains.Service
{
    [Reentrant]
    public class ScheduleTaskGrain : MainGrain, IScheduleTaskGrain
    {
        private readonly IScheduleTaskRepository _scheduleRepository;

        public ScheduleTaskGrain(IScheduleTaskRepository scheduleTaskRepository)
        {
            this._scheduleRepository = scheduleTaskRepository;
        }

        public async Task<ApiResultPage<ScheduleTaskVO>> GetPageSync(ScheduleTaskDTO model)
        {
            var expression = model.GetExpression();
            var orders = model.GetOrder();
            var count = await _scheduleRepository.CountAsync(expression);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPage<ScheduleTaskVO>("无数据！");
            }

            var entities = await _scheduleRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);
            var scheduleTaskVOs = this.ObjectMapper.Map<List<ScheduleTaskVO>>(entities);
            return ApiResultUtil.IsSuccess(scheduleTaskVOs, count);
        }
    }
}
