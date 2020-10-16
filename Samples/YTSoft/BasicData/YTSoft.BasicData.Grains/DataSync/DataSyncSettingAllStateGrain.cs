using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Dapper.Expressions;
using Kaneko.Server.Orleans.Grains;
using Kaneko.Server.Orleans.Services;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.BasicData.Grains.DataSync.Repository;
using YTSoft.BasicData.IGrains.DataDictionary.Model;
using YTSoft.BasicData.IGrains.DataSync;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.Grains.DataSync
{
    [Reentrant]
    public class DataSyncSettingAllStateGrain : StateGrain<Guid, DataSyncAllState>, IDataSyncSettingAllStateGrain
    {
        private readonly IColumnInfoRepository _columnInfoRepository;
        private readonly ITableInfoRepository _tableInfoRepository;
        private readonly ISystemInfoRepository _systemInfoRepository;

        public DataSyncSettingAllStateGrain(ISystemInfoRepository systemInfoRepository, ITableInfoRepository tableInfoRepository, IColumnInfoRepository columnInfoRepository)
        {
            _columnInfoRepository = columnInfoRepository;
            _tableInfoRepository = tableInfoRepository;
            _systemInfoRepository = systemInfoRepository;
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
        protected override async Task<DataSyncAllState> OnReadFromDbAsync()
        {
            DataSyncAllState state = new DataSyncAllState
            {
                SystemInfos = (await _systemInfoRepository.GetAllAsync()).ToList(),
                TableInfos = (await _tableInfoRepository.GetAllAsync()).ToList(),
                ColumnInfos = (await _columnInfoRepository.GetAllAsync()).ToList()
            };
            return state;
        }

        public async Task<ApiResult> AddAsync(SystemInfoAllDTO model)
        {
            //参数校验
            if (!model.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            if (State.SystemInfos.Any(m => m.SystemName == model.SystemName || m.DbConnection == model.DbConnection))
            {
                return ApiResultUtil.IsFailed("系统或数据库配置已存在！");
            }

            long systemId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();

            var systemInfoDO = this.ObjectMapper.Map<SystemInfoDO>(model);
            systemInfoDO.Id = systemId;
            systemInfoDO.CreateDate = System.DateTime.Now;
            systemInfoDO.ModityDate = System.DateTime.Now;

            List<TableInfoDO> tableInfos = new List<TableInfoDO>();
            List<ColumnInfoDO> columnInfos = new List<ColumnInfoDO>();

            foreach (var table in model.Tables)
            {
                if (tableInfos.Any(mbox => mbox.TableName == table.TableName)) { return ApiResultUtil.IsFailed($"表名称【{table.TableName}】重复！"); }

                long tableId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
                var tableInfoDO = this.ObjectMapper.Map<TableInfoDO>(table);
                tableInfoDO.SystemId = systemId;
                tableInfoDO.Id = tableId;
                tableInfoDO.CreateDate = System.DateTime.Now;
                tableInfoDO.ModityDate = System.DateTime.Now;

                foreach (var column in table.Columns)
                {
                    if (columnInfos.Any(mbox => mbox.TableId == tableId && mbox.SelfColumn == column.SelfColumn)) { return ApiResultUtil.IsFailed($"列名称【{column.SelfColumn}】重复！"); }

                    long columnId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
                    var columnInfo = this.ObjectMapper.Map<ColumnInfoDO>(column);
                    columnInfo.TableId = tableId;
                    columnInfo.Id = columnId;
                    columnInfo.CreateDate = System.DateTime.Now;
                    columnInfo.ModityDate = System.DateTime.Now;

                    columnInfos.Add(columnInfo);
                }

                tableInfos.Add(tableInfoDO);
            }

            var bRet = await _systemInfoRepository.BeginTransactionAsync(async transaction =>
            {
                var result = await _systemInfoRepository.AddAsync(systemInfoDO);

                _tableInfoRepository.Transaction = transaction;
                result &= await _tableInfoRepository.AddAsync(tableInfos.ToArray());

                _columnInfoRepository.Transaction = transaction;
                result &= await _columnInfoRepository.AddAsync(columnInfos.ToArray());
                transaction.Commit();
                return result;
            });

            var state = this.State;
            state.SystemInfos.Add(systemInfoDO);
            state.TableInfos.AddRange(tableInfos);
            state.ColumnInfos.AddRange(columnInfos);

            //刷新缓存
            var groupCodes = tableInfos.Select(mbox => mbox.GroupCode).Distinct();
            await RefrshGroupState(groupCodes.ToList());

            await this.Persist(Kaneko.Core.Contract.ProcessAction.Create, state);
            return bRet ? ApiResultUtil.IsSuccess() : ApiResultUtil.IsFailed("数据保存失败！");
        }

        public Task<ApiResult<DataSyncAllState>> GetAsync()
        {
            return Task.FromResult(ApiResultUtil.IsSuccess(this.State));
        }

        public async Task<ApiResult> UpdateSystemAsync(SystemInfoDO model)
        {
            var state = this.State;
            if (state.SystemInfos.Any(m => m.Id == model.Id))
            {
                var systemInfoDO = state.SystemInfos.First(mbox => mbox.Id == model.Id);

                string oldConnection = systemInfoDO.DbConnection;

                systemInfoDO.IsDel = model.IsDel;
                systemInfoDO.SystemName = string.IsNullOrWhiteSpace(model.SystemName) ? systemInfoDO.SystemName : model.SystemName;
                systemInfoDO.DbConnection = string.IsNullOrWhiteSpace(model.DbConnection) ? systemInfoDO.DbConnection : model.DbConnection;
                systemInfoDO.Desc = string.IsNullOrWhiteSpace(model.Desc) ? systemInfoDO.Desc : model.Desc;
                systemInfoDO.Version++;
                systemInfoDO.ModityDate = DateTime.Now;
                systemInfoDO.Firm = string.IsNullOrWhiteSpace(model.Firm) ? systemInfoDO.Firm : model.Firm;
                systemInfoDO.Line = string.IsNullOrWhiteSpace(model.Line) ? systemInfoDO.Line : model.Line;

                bool bRet = await _systemInfoRepository.SetAsync(systemInfoDO);
                if (!bRet) { return ApiResultUtil.IsFailed("更新失败！"); }

                //if (oldConnection != model.DbConnection || oldDbType != model.DbType)
                {
                    //修改了数据库配置需要刷新缓存
                    var groupCodes = state.TableInfos.Where(m => m.SystemId == model.Id).Select(mbox => mbox.GroupCode).Distinct();
                    await RefrshGroupState(groupCodes.ToList());
                }

                await this.Persist(Kaneko.Core.Contract.ProcessAction.Update, this.State);
            }

            return ApiResultUtil.IsFailed($"系统ID【{model.Id}】不存在！");
        }

        public async Task<ApiResult> AddTableAsync(List<TableInfoDTO> model)
        {
            //参数校验
            if (!model.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            List<TableInfoDO> tableInfos = new List<TableInfoDO>();
            List<ColumnInfoDO> columnInfos = new List<ColumnInfoDO>();

            foreach (var table in model)
            {
                if (tableInfos.Any(mbox => mbox.TableName == table.TableName)) { return ApiResultUtil.IsFailed($"表名称【{table.TableName}】重复！"); }

                long tableId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
                var tableInfoDO = this.ObjectMapper.Map<TableInfoDO>(table);
                tableInfoDO.Id = tableId;
                tableInfoDO.CreateDate = DateTime.Now;
                tableInfoDO.ModityDate = DateTime.Now;

                foreach (var column in table.Columns)
                {
                    if (columnInfos.Any(mbox => mbox.TableId == tableId && mbox.SelfColumn == column.SelfColumn)) { return ApiResultUtil.IsFailed($"列名称【{column.SelfColumn}】重复！"); }

                    long columnId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
                    var columnInfo = this.ObjectMapper.Map<ColumnInfoDO>(column);
                    columnInfo.TableId = tableId;
                    columnInfo.Id = columnId;
                    columnInfo.CreateDate = System.DateTime.Now;
                    columnInfo.ModityDate = System.DateTime.Now;

                    columnInfos.Add(columnInfo);
                }

                tableInfos.Add(tableInfoDO);
            }

            var bRet = await _tableInfoRepository.BeginTransactionAsync(async transaction =>
            {
                var result = await _tableInfoRepository.AddAsync(tableInfos.ToArray());
                result &= await _columnInfoRepository.AddAsync(columnInfos.ToArray());
                transaction.Commit();
                return result;
            });

            if (!bRet) { return ApiResultUtil.IsFailed("数据保存失败！"); }

            var state = this.State;
            state.TableInfos.AddRange(tableInfos);
            state.ColumnInfos.AddRange(columnInfos);

            //刷新缓存
            var groupCodes = tableInfos.Select(mbox => mbox.GroupCode).Distinct();
            await RefrshGroupState(groupCodes.ToList());

            await this.Persist(Kaneko.Core.Contract.ProcessAction.Create, state);
            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult> UpdateTableAsync(TableInfoDO model)
        {
            var state = this.State;
            if (state.TableInfos.Any(m => m.Id == model.Id))
            {
                var tableInfoDO = state.TableInfos.First(mbox => mbox.Id == model.Id);
                tableInfoDO.IsDel = model.IsDel;
                tableInfoDO.TableName = string.IsNullOrWhiteSpace(model.TableName) ? tableInfoDO.TableName : model.TableName;
                tableInfoDO.GroupCode = string.IsNullOrWhiteSpace(model.GroupCode) ? tableInfoDO.GroupCode : model.GroupCode;
                tableInfoDO.Desc = string.IsNullOrWhiteSpace(model.Desc) ? tableInfoDO.Desc : model.Desc;
                tableInfoDO.Version++;
                tableInfoDO.ModityDate = DateTime.Now;

                bool bRet = await _tableInfoRepository.SetAsync(tableInfoDO);

                if (!bRet) { return ApiResultUtil.IsFailed("更新失败！"); }

                //刷新缓存
                var groupCodes = new List<string> { tableInfoDO.GroupCode };
                await RefrshGroupState(groupCodes.ToList());

                await this.Persist(Kaneko.Core.Contract.ProcessAction.Update, this.State);
            }

            return ApiResultUtil.IsFailed($"表ID【{model.Id}】不存在！");
        }

        public async Task<ApiResult> AddColumnAsync(List<ColumnInfoDTO> model)
        {
            //参数校验
            if (!model.IsValid(out string exMsg)) { return ApiResultUtil.IsFailed(exMsg); }

            List<ColumnInfoDO> columnInfos = new List<ColumnInfoDO>();
            List<string> groupCodes = new List<string>();
            foreach (var column in model)
            {
                if (columnInfos.Any(mbox => mbox.SelfColumn == column.SelfColumn)) { return ApiResultUtil.IsFailed($"列名称【{column.SelfColumn}】重复！"); }
                if (!this.State.TableInfos.Any(mbox => mbox.Id == column.TableId)) { return ApiResultUtil.IsFailed($"表ID【{column.TableId}】不存在！"); }

                groupCodes.Add(this.State.TableInfos.First(mbox => mbox.Id == column.TableId).GroupCode);

                long columnId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewLongID();
                var columnInfo = this.ObjectMapper.Map<ColumnInfoDO>(column);
                columnInfo.Id = columnId;
                columnInfo.CreateDate = System.DateTime.Now;
                columnInfo.ModityDate = System.DateTime.Now;
                columnInfos.Add(columnInfo);
            }

            bool bRet = await _columnInfoRepository.AddAsync(columnInfos.ToArray());
            if (!bRet) { return ApiResultUtil.IsFailed("数据保存失败！"); }

            var state = this.State;
            state.ColumnInfos.AddRange(columnInfos);

            //刷新缓存
            await RefrshGroupState(groupCodes.Distinct().ToList());

            await this.Persist(Kaneko.Core.Contract.ProcessAction.Create, state);
            return ApiResultUtil.IsSuccess();
        }

        public async Task<ApiResult> UpdateColumnAsync(ColumnInfoDO model)
        {
            var state = this.State;
            if (state.ColumnInfos.Any(m => m.Id == model.Id))
            {
                var columnInfo = state.ColumnInfos.First(mbox => mbox.Id == model.Id);
                columnInfo.IsDel = model.IsDel;
                columnInfo.SelfColumn = string.IsNullOrWhiteSpace(model.SelfColumn) ? columnInfo.SelfColumn : model.SelfColumn;
                columnInfo.ModelColumn = string.IsNullOrWhiteSpace(model.ModelColumn) ? columnInfo.ModelColumn : model.ModelColumn;
                columnInfo.Desc = string.IsNullOrWhiteSpace(model.Desc) ? columnInfo.Desc : model.Desc;
                columnInfo.SortNo = model.SortNo > 0 ? model.SortNo : columnInfo.SortNo;
                columnInfo.DisplayName = string.IsNullOrWhiteSpace(model.DisplayName) ? columnInfo.DisplayName : model.DisplayName;
                columnInfo.Version++;
                columnInfo.ModityDate = DateTime.Now;

                bool bRet = await _columnInfoRepository.SetAsync(columnInfo);

                if (!bRet) { return ApiResultUtil.IsFailed("更新失败！"); }

                //刷新缓存
                var groupCodes = new List<string> { this.State.TableInfos.First(mbox => mbox.Id == columnInfo.TableId).GroupCode };
                await RefrshGroupState(groupCodes.ToList());

                await this.Persist(Kaneko.Core.Contract.ProcessAction.Update, this.State);
            }

            return ApiResultUtil.IsFailed($"列ID【{model.Id}】不存在！");
        }

        private async Task RefrshGroupState(List<string> groupCodes)
        {
            foreach (var groupCode in groupCodes)
            {
                var dataSyncGroupState = new DataSyncGroupState();

                if (this.State.TableInfos.Any(mbox => mbox.GroupCode == groupCode && mbox.IsDel == 0))
                {
                    var tableInfoDOs1 = this.State.TableInfos.Where(m => m.GroupCode == groupCode && m.IsDel == 0);
                    var tableInfoStates = this.ObjectMapper.Map<List<TableInfoState>>(tableInfoDOs1);

                    foreach (var table in tableInfoDOs1)
                    {
                        TableInfoState tableInfoState = this.ObjectMapper.Map<TableInfoState>(table);

                        var systemInfoDO = this.State.SystemInfos.FirstOrDefault(mbox => mbox.Id == table.SystemId && mbox.IsDel == 0);
                        tableInfoState.SystemInfo = this.ObjectMapper.Map<SystemInfoState>(systemInfoDO);

                        var columnInfoDOs = this.State.ColumnInfos.Where(mbox => mbox.TableId == table.Id && mbox.IsDel == 0);
                        tableInfoState.Columns = this.ObjectMapper.Map<List<ColumnInfoState>>(columnInfoDOs.AsEnumerable());

                        dataSyncGroupState.Tables.Add(tableInfoState);
                    }
                }

                await this.GrainFactory.GetGrain<IDataSyncSettingGroupStateGrain>(groupCode).ReinstantiateState(dataSyncGroupState);
            }
        }

        /// <summary>
        /// 重置状态数据
        /// </summary>
        /// <returns></returns>
        public override async Task<ApiResult> ReinstantiateState(DataSyncAllState state = default)
        {
            var result = await base.ReinstantiateState();

            //刷新缓存
            var groupCodes = this.State.TableInfos.Select(m => m.GroupCode).Distinct().ToList();
            await RefrshGroupState(groupCodes.ToList());

            return result;
        }
    }
}
