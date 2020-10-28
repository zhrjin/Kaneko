using Kaneko.Core.ApiResult;
using Kaneko.Server.Orleans.Grains;
using System.Threading.Tasks;
using Orleans.Concurrency;
using YTSoft.BasicData.IGrains.DataDictionary;
using YTSoft.BasicData.Grains.DataDictionary.Repository;
using YTSoft.BasicData.IGrains.DataDictionary.Model;
using Kaneko.Dapper.Expressions;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Services;
using System.Linq;
using Orleans;
using System.Collections.Generic;
using System.Diagnostics;
using TYSoft.Common.Model.Test;
using TYSoft.Common.Model.EventBus;
using System.Net.Http;
using System;
using Kaneko.Core.Utils;

namespace YTSoft.BasicData.Grains.DataDictionary
{
    [Reentrant]
    public class DictDataTypeStateGrain : StateGrain<string, DictState>, IDictDataTypeStateGrain
    {
        private readonly IDictRepository _dictRepository;
        private readonly IOrleansClient _orleansClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public DictDataTypeStateGrain(IDictRepository dictRepository, IOrleansClient orleansClient, IHttpClientFactory httpClientFactory)
        {
            this._dictRepository = dictRepository;
            this._orleansClient = orleansClient;
            _httpClientFactory = httpClientFactory;
        }


        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

#if DEBUG //解决开发环境Redis缓存和数据库数据不一致
            if (GrainDataState.Init == this.State.GrainDataState)
            {
                var onReadDbTask = OnReadFromDbAsync();
                if (!onReadDbTask.IsCompletedSuccessfully)
                    await onReadDbTask;
                var result = onReadDbTask.Result;
                if (result != null)
                {
                    State = result;
                    State.GrainDataState = GrainDataState.Loaded;
                    await WriteStateAsync();
                }
            }
#endif
        }

        /// <summary>
        /// 初始化状态数据
        /// </summary>
        /// <returns></returns>
        protected override async Task<DictState> OnReadFromDbAsync()
        {
            var dbResult = await _dictRepository.GetAllAsync(oo => oo.DataType == this.GrainId, isMaster: false);
            DictState state = new DictState
            {
                Balance = dbResult.ToList()
            };
            return state;
        }

        public async Task<ApiResult> AddAsync(SubmitDTO<DictDTO> model)
        {
            var dto = model.Data;
            //参数校验
            if (!dto.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            //转换为数据库实体
            DictDO dictDO = this.ObjectMapper.Map<DictDO>(dto);

            long newId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();

            dictDO.Id = newId;
            dictDO.IsDel = 0;
            dictDO.Version = 1;
            dictDO.CreateBy = model.UserId;
            dictDO.CreateDate = System.DateTime.Now;
            dictDO.ModityBy = model.UserId;
            dictDO.ModityDate = System.DateTime.Now;

            string attribute = System.Guid.NewGuid().ToString();

            //测试先去掉数据写数
            //await PublishAsync(new TestModel
            //{
            //    Id = System.Guid.NewGuid().ToString(),
            //    CreateDate = System.DateTime.Now,
            //    Attribute01 = attribute,
            //    Attribute02 = attribute,
            //    Attribute03 = attribute,
            //    Attribute04 = attribute,
            //    Attribute05 = attribute,
            //    Attribute06 = attribute,
            //    Attribute07 = attribute,
            //    Attribute08 = attribute,
            //    Attribute09 = attribute,
            //    Attribute10 = attribute,
            //    Attribute11 = attribute,
            //    Attribute12 = attribute,
            //    Attribute13 = attribute
            //});

            bool bRet = await _dictRepository.AddAsync(dictDO);
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }

            if (this.GrainId != "TY22")
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://127.0.0.1:6114");
                Dictionary<string, string> postForm = new Dictionary<string, string>
                {
                    { "Data.DataType", "TY22" },
                    { "Data.DataCode", "A001"},
                    { "Data.DataValue", "V001" },
                    { "Data.DataDesc", "测试" },
                    { "Data.SortNo", "1" },
                    { "Data.Firm", "YT001" }
                };
                var task = await client.PostKanekoAsync("/BasicData/api/Dict/add", "", null, postForm);
            }

            //更新服务状态
            DictState scheduleTaskState = this.State;
            scheduleTaskState.Balance.Add(dictDO);

            await this.Persist(ProcessAction.Create, scheduleTaskState);
            return ApiResultUtil.IsSuccess(newId.ToString());
        }

        public async Task<ApiResult> DeleteAsync(long id)
        {
            if (id <= 0) { return ApiResultUtil.IsFailed("主键ID不能为空！"); }
            if (!this.State.Balance.Any(m => m.Id == id)) { return ApiResultUtil.IsFailed("不存在该字典！"); }

            var currentState = this.State.Balance.First(m => m.Id == id);

            bool bRet = await _dictRepository.SetAsync(() => new { is_del = 1, version = (currentState.Version + 1), modity_date = System.DateTime.Now }, oo => oo.Id == id);
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            currentState.IsDel = 1;
            currentState.ModityDate = System.DateTime.Now;
            currentState.Version++;
            await this.Persist(ProcessAction.Update, this.State);
            return ApiResultUtil.IsSuccess();
        }

        public Task<ApiResult<DictVO>> GetAsync(long id)
        {
            if (this.State.Balance.Any(m => m.Id == id))
            {
                var dictDO = this.State.Balance.First(m => m.Id == id);
                DictVO dictVO = this.ObjectMapper.Map<DictVO>(dictDO);
                return Task.FromResult(ApiResultUtil.IsSuccess(dictVO));
            }

            return Task.FromResult(ApiResultUtil.IsFailed<DictVO>("无数据！"));
        }

        public Task<ApiResultList<DictVO>> GetListAsync()
        {
            var balance = this.State.Balance;
            List<DictVO> dictVOs = this.ObjectMapper.Map<List<DictVO>>(balance);
            return Task.FromResult(ApiResultUtil.IsSuccess<DictVO>(dictVOs));
        }

        public async Task<ApiResult> UpdateAsync(SubmitDTO<DictDTO> model)
        {
            var dto = model.Data;
            if (dto.Id <= 0) { return ApiResultUtil.IsFailed("主键ID不能为空！"); }
            if (!this.State.Balance.Any(m => m.Id == dto.Id)) { return ApiResultUtil.IsFailed("不存在该字典！"); }

            var currentState = this.State.Balance.First(m => m.Id == dto.Id);

            bool bRet = await _dictRepository.SetAsync(() => new
            {
                is_del = 1,
                data_desc = dto.DataDesc,
                data_value = dto.DataValue,
                version = (currentState.Version + 1),
                modity_date = System.DateTime.Now
            }, oo => oo.Id == dto.Id);

            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }

            currentState.DataValue = dto.DataValue;
            currentState.DataDesc = dto.DataDesc;
            currentState.IsDel = 1;
            currentState.ModityDate = System.DateTime.Now;
            currentState.Version++;
            await this.Persist(ProcessAction.Update, this.State);
            return ApiResultUtil.IsSuccess();
        }
    }
}