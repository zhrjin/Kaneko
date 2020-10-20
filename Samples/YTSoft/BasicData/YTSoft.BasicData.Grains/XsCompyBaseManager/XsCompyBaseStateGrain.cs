using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using System.Linq;
using YTSoft.BasicData.IGrains.XsCompyBaseManager;
using YTSoft.BasicData.Grains.XsCompyBaseManager.Repository;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;
using Kaneko.Core.Contract;
using System;
using System.Linq.Expressions;

namespace YTSoft.BasicData.Grains.XsCompyBaseManager
{
    [Reentrant]
    public class XsCompyBaseStateGrain : ListStateGrain<string, XsCompyBaseState>, IXsCompyBaseStateGrain
    {
        private readonly IXsCompyBaseRepository _xsCompyBaseRepository;

        public XsCompyBaseStateGrain(IXsCompyBaseRepository xsCompyBaseRepository)
        {
            this._xsCompyBaseRepository = xsCompyBaseRepository;
        }

        /// <summary>
        /// 初始化状态数据
        /// </summary>
        /// <returns></returns>
        protected override async Task<XsCompyBaseState> OnReadFromDbAsync()
        {
            string firm = this.GrainId;
            Expression<Func<XsCompyBaseDO, bool>> expression = oo => oo.XOB_Firm == firm;
            var dbResult = await _xsCompyBaseRepository.GetAllAsync(expression, isMaster: false);
            var state = new XsCompyBaseState
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
        public Task<ApiResultList<XsCompyBaseDO>> GetAllSync(XsCompyBaseDTO model)
        {
            if (this.State.Balance.Any(mbox => model.Ids.Contains(mbox.XOB_ID)))
            {
                return Task.FromResult(ApiResultUtil.IsSuccess<XsCompyBaseDO>(this.State.Balance.Where(mbox => model.Ids.Contains(mbox.XOB_ID))?.ToList()));
            }
            return Task.FromResult(ApiResultUtil.IsFailedList<XsCompyBaseDO>("无数据！"));
        }
    }
}