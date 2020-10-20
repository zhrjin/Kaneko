using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.PbBasicFirmManager;
using YTSoft.BasicData.Grains.PbBasicFirmManager.Repository;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.Grains.PbBasicFirmManager
{
    [Reentrant]
    public class PbBasicFirmStateGrain : ListStateGrain<string, PbBasicFirmState>, IPbBasicFirmStateGrain
    {
        private readonly IPbBasicFirmRepository _pbBasicFirmRepository;

        public PbBasicFirmStateGrain(IPbBasicFirmRepository pbBasicFirmRepository)
        {
            this._pbBasicFirmRepository = pbBasicFirmRepository;
        }

        /// <summary>
        /// 初始化状态数据
        /// </summary>
        /// <returns></returns>
        protected override async Task<PbBasicFirmState> OnReadFromDbAsync()
        {
            var dbResult = await _pbBasicFirmRepository.GetAllAsync();
            var state = new PbBasicFirmState
            {
                Balance = dbResult.ToList(),
                ExpiresAt = System.DateTime.UtcNow.AddHours(2),
                GrainDataState = GrainDataState.Loaded
            };
            return state;
        }

        /// <summary>
        /// 失活时判断缓存数据是否过期，若过期则重新加载
        /// </summary>
        /// <returns></returns>
        public override Task OnDeactivateAsync()
        {
            var expiresAt = this.State.ExpiresAt;
            if (expiresAt.HasValue && (expiresAt.Value - System.DateTime.UtcNow).TotalMinutes < 2)
            {
                return this.ReinstantiateState();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<ApiResultList<PbBasicFirmDO>> GetAllSync(PbBasicFirmDTO model)
        {
            if (this.State.Balance.Any(mbox => model.Ids.Contains(mbox.PBF_ID)))
            {
                return Task.FromResult(ApiResultUtil.IsSuccess<PbBasicFirmDO>(this.State.Balance.Where(mbox => model.Ids.Contains(mbox.PBF_ID))?.ToList()));
            }
            return Task.FromResult(ApiResultUtil.IsFailedList<PbBasicFirmDO>("无数据！"));
        }
    }
}