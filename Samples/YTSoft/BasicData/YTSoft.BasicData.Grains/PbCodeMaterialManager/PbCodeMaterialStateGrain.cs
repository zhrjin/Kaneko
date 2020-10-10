using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager;
using YTSoft.BasicData.Grains.PbCodeMaterialManager.Repository;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;
using Kaneko.Core.Contract;
using System.Linq.Expressions;
using System;

namespace YTSoft.BasicData.Grains.PbCodeMaterialManager
{
    [Reentrant]
    public class PbCodeMaterialStateGrain : ListStateGrain<string, PbCodeMaterialState>, IPbCodeMaterialStateGrain
    {
        private readonly IPbCodeMaterialRepository _pbCodeMaterialRepository;

        public PbCodeMaterialStateGrain(IPbCodeMaterialRepository pbCodeMaterialRepository)
        {
            this._pbCodeMaterialRepository = pbCodeMaterialRepository;
        }

        /// <summary>
        /// 初始化状态数据
        /// </summary>
        /// <returns></returns>
        protected override async Task<PbCodeMaterialState> OnReadFromDbAsync()
        {
            string firm = this.GrainId;
            Expression<Func<PbCodeMaterialDO, bool>> expression = oo => oo.PCM_Firm == firm;
            var dbResult = await _pbCodeMaterialRepository.GetAllAsync(expression, isMaster: false);
            var state = new PbCodeMaterialState
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
        public Task<ApiResultList<PbCodeMaterialDO>> GetAllSync(PbCodeMaterialDTO model)
        {
            if (this.State.Balance.Any(mbox => model.Ids.Contains(mbox.PCM_ID)))
            {
                return Task.FromResult(ApiResultUtil.IsSuccess(this.State.Balance.Where(mbox => model.Ids.Contains(mbox.PCM_ID))?.ToList()));
            }
            return Task.FromResult(ApiResultUtil.IsFailedList<PbCodeMaterialDO>("无数据！"));
        }
    }
}