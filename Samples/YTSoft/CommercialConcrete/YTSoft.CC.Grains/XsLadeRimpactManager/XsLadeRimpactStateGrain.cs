using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Kaneko.Server.Orleans.Services;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbBasicFirmManager;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;
using YTSoft.CC.Grains.XsLadeRimpactManager.Repository;
using YTSoft.CC.IGrains.XsLadeBaseManager;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;
using YTSoft.CC.IGrains.XsLadeRimpactManager;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;
using YTSoft.CC.IGrains.XsReceReceivableManager;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.Grains.XsLadeRimpactManager
{
    [Reentrant]
    public class XsLadeBaseStateGrain : StateGrain<string, XsLadeRimpactState>, IXsLadeRimpactStateGrain
    {
        private readonly IXsLadeRimpactRepository _xsLadeRimpactRepository;
        private readonly IOrleansClient _orleansClient;

        public XsLadeBaseStateGrain(IXsLadeRimpactRepository xsLadeRimpactRepository, IOrleansClient orleansClient)
        {
            this._xsLadeRimpactRepository = xsLadeRimpactRepository;
            this._orleansClient = orleansClient;
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
        protected override async Task<XsLadeRimpactState> OnReadFromDbAsync()
        {
            var dbResult = await _xsLadeRimpactRepository.GetAsync(oo => oo.Id == this.GrainId, isMaster: false);
            var result = this.ObjectMapper.Map<XsLadeRimpactState>(dbResult);
            return result;
        }

        /// <summary>
        /// 查看发货单作废
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<XsLadeRimpactVO>> GetAsync()
        {
            var state = this.State;
            var xsLadeRimpactVO = this.ObjectMapper.Map<XsLadeRimpactVO>(state);

            //获取企业数据
            if (!string.IsNullOrEmpty(xsLadeRimpactVO.XLR_Firm))
            {
                var itemResult = await _orleansClient.GetGrain<IPbBasicFirmStateGrain>(GrainIdKey.UtcUIDGrainKey.ToString()).GetAllSync(new PbBasicFirmDTO
                {
                    Ids = new List<string> { xsLadeRimpactVO.XLR_Firm }
                });
                if (itemResult.Success)
                {
                    xsLadeRimpactVO.XLR_FirmName = itemResult.Data.FirstOrDefault().PBF_ShortName;
                }
            }

            //获取发货单编码
            if (!string.IsNullOrEmpty(xsLadeRimpactVO.XLR_Lade))
            {
                var itemResult = await GrainFactory.GetGrain<IXsLadeBaseStateGrain>(xsLadeRimpactVO.XLR_Lade).GetAsync();
                if (itemResult.Success)
                {
                    xsLadeRimpactVO.XLB_LadeId = itemResult.Data.XLB_LadeId;
                }
            }

            return await Task.FromResult(ApiResultUtil.IsSuccess(xsLadeRimpactVO));
        }

        /// <summary>
        /// 新增发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddAsync(SubmitDTO<XsLadeRimpactDTO> model)
        {
            var dto = model.Data;
            #region 插入发货单作废
            //查对应发货单
            ApiResult<XsLadeBaseVO> xsLadeBaseResult = await this.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(dto.XLR_Lade).GetAsync();
            XsLadeBaseDTO xsLadeBaseDTO = xsLadeBaseResult.Success ? this.ObjectMapper.Map<XsLadeBaseDTO>(xsLadeBaseResult.Data) : null;
            //插入发货单作废
            XsLadeRimpactDO xsLadeRimpactDO = this.ObjectMapper.Map<XsLadeRimpactDO>(dto);
            xsLadeRimpactDO.XLR_Total = xsLadeBaseDTO.XLB_FactTotal;
            xsLadeRimpactDO.CreateBy = model.UserId;
            xsLadeRimpactDO.CreateByName = model.UserName;
            xsLadeRimpactDO.CreateDate = System.DateTime.Now;
            xsLadeRimpactDO.ModityBy = model.UserId;
            xsLadeRimpactDO.ModityByName = model.UserName;
            xsLadeRimpactDO.ModityDate = System.DateTime.Now;
            bool bRet = await _xsLadeRimpactRepository.AddAsync(xsLadeRimpactDO); ;
            if (!bRet) { return ApiResultUtil.IsFailed("数据插入失败！"); }
            #endregion

            if (dto.XLR_Type == "101")//本月
            {
                #region 更新发货单状态
                xsLadeBaseDTO.XLB_Status = "2";
                await this.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(xsLadeBaseDTO.Id).UpdateAsync(new SubmitDTO<XsLadeBaseDTO>
                {
                    Data = xsLadeBaseDTO,
                    UserId = model.UserId,
                    UserName = model.UserName
                });
                #endregion

                #region 删除应收款
                var xsReceReceivableResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
                {
                    XRC_BillID = dto.XLR_Lade,
                    XRC_Origins = { "102", "103", "104" }
                });
                if (xsReceReceivableResult.Success && xsReceReceivableResult.Data.Count > 0)
                {
                    foreach (XsReceReceivableDO xsReceReceivableDO in xsReceReceivableResult.Data)
                    {
                        xsReceReceivableDO.IsDel = 1;
                        XsReceReceivableDTO xsReceReceivableDTO = this.ObjectMapper.Map<XsReceReceivableDTO>(xsReceReceivableDO);
                        await this.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(xsReceReceivableDTO.Id).UpdateAsync(new SubmitDTO<XsReceReceivableDTO>
                        {
                            Data = xsReceReceivableDTO,
                            UserId = model.UserId,
                            UserName = model.UserName
                        });
                    }
                }
                #endregion
            }
            else if (dto.XLR_Type == "102")//跨月
            {
                #region 插入调整应收款
                var xsReceReceivableResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
                {
                    XRC_BillID = dto.XLR_Lade,
                    XRC_Origins = { "102", "103", "104" }
                });
                if (xsReceReceivableResult.Success && xsReceReceivableResult.Data.Count > 0)
                {
                    foreach (XsReceReceivableDO xsReceReceivableDO in xsReceReceivableResult.Data)
                    {
                        XsReceReceivableDTO xsReceReceivableDTO = new XsReceReceivableDTO
                        {
                            Id = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID(),
                            XRC_SetDate = xsLadeRimpactDO.XLR_SetDate,
                            XRC_BillID = xsLadeRimpactDO.Id,//作废主键
                            XRC_Origin = "4" + xsReceReceivableDO.XRC_Origin,
                            XRC_Total = -xsReceReceivableDO.XRC_Total,
                            XRC_Firm = xsLadeRimpactDO.XLR_Firm
                        };
                        await this.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(xsReceReceivableDTO.Id).AddAsync(new SubmitDTO<XsReceReceivableDTO>
                        {
                            Data = xsReceReceivableDTO,
                            UserId = model.UserId,
                            UserName = model.UserName
                        });
                    }
                }
                #endregion
            }

            //更新服务状态
            XsLadeRimpactState xsLadeRimpactState = this.ObjectMapper.Map<XsLadeRimpactState>(xsLadeRimpactDO);

            await this.Persist(ProcessAction.Create, xsLadeRimpactState);

            return ApiResultUtil.IsSuccess("处理成功！");
        }

        /// <summary>
        /// 删除发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> DelAsync(SubmitDTO<XsLadeRimpactDTO> model)
        {
            var dto = model.Data;

            if (dto.Version != this.State.Version) { return ApiResultUtil.IsFailed("数据已被修改，请重新再加载！"); }
            dto.Version++;

            #region 删除发货单作废
            //查对应发货单
            var xsLadeBaseResult = await this.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(this.State.XLR_Lade).GetAsync();
            XsLadeBaseDTO xsLadeBaseDTO = xsLadeBaseResult.Success ? this.ObjectMapper.Map<XsLadeBaseDTO>(xsLadeBaseResult.Data) : null;
            //删除发货单作废
            XsLadeRimpactState xsLadeRimpactState = this.State;
            xsLadeRimpactState.IsDel = 1;
            xsLadeRimpactState.ModityBy = model.UserId;
            xsLadeRimpactState.ModityByName = model.UserName;
            xsLadeRimpactState.ModityDate = System.DateTime.Now;
            xsLadeRimpactState.Version = dto.Version;
            XsLadeRimpactDO xsLadeRimpactDO = this.ObjectMapper.Map<XsLadeRimpactDO>(xsLadeRimpactState);
            bool bRet = await _xsLadeRimpactRepository.SetAsync(xsLadeRimpactDO); ;
            if (!bRet) { return ApiResultUtil.IsFailed("数据更新失败！"); }
            #endregion

            //查对应应收款
            IList<XsReceReceivableDO> xsReceReceivableDOs = new List<XsReceReceivableDO>();
            if (xsLadeRimpactState.XLR_Type == "101")//本月
            {
                #region 更新发货单状态
                xsLadeBaseDTO.XLB_Status = "1";
                await this.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(xsLadeBaseDTO.Id).UpdateAsync(new SubmitDTO<XsLadeBaseDTO>
                {
                    Data = xsLadeBaseDTO,
                    UserId = model.UserId,
                    UserName = model.UserName
                });
                #endregion

                #region 还原应收款
                var xsReceReceivableResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
                {
                    XRC_BillID = dto.XLR_Lade,
                    XRC_Origins = { "102", "103", "104" }
                });
                if (xsReceReceivableResult.Success && xsReceReceivableResult.Data.Count > 0)
                {
                    foreach (XsReceReceivableDO xsReceReceivableDO in xsReceReceivableResult.Data)
                    {
                        xsReceReceivableDO.IsDel = 0;
                        XsReceReceivableDTO xsReceReceivableDTO = this.ObjectMapper.Map<XsReceReceivableDTO>(xsReceReceivableDO);
                        await this.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(xsReceReceivableDTO.Id).UpdateAsync(new SubmitDTO<XsReceReceivableDTO>
                        {
                            Data = xsReceReceivableDTO,
                            UserId = model.UserId,
                            UserName = model.UserName
                        });
                    }
                }
                #endregion
            }
            else if (xsLadeRimpactState.XLR_Type == "102")//跨月
            {
                #region 删除调整应收款
                var xsReceReceivableResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
                {
                    XRC_BillID = dto.Id,
                    XRC_Origins = { "4102", "4103", "4104" }
                });
                if (xsReceReceivableResult.Success && xsReceReceivableResult.Data.Count > 0)
                {
                    foreach (XsReceReceivableDO xsReceReceivableDO in xsReceReceivableResult.Data)
                    {
                        xsReceReceivableDO.IsDel = 1;
                        XsReceReceivableDTO xsReceReceivableDTO = this.ObjectMapper.Map<XsReceReceivableDTO>(xsReceReceivableDO);
                        await this.GrainFactory.GetGrain<IXsReceReceivableStateGrain>(xsReceReceivableDTO.Id).UpdateAsync(new SubmitDTO<XsReceReceivableDTO>
                        {
                            Data = xsReceReceivableDTO,
                            UserId = model.UserId,
                            UserName = model.UserName
                        });
                    }
                }
                #endregion
            }

            await this.Persist(ProcessAction.Update, xsLadeRimpactState);

            return ApiResultUtil.IsSuccess("处理成功！");
        }

        /// <summary>
        /// 提交发货单作废
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SubmitAsync(Guid transactionId, SubmitDTO<XsLadeRimpactDTO> model)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                return;
            }

            var dto = model.Data;

            //Add
            XsLadeRimpactDO xsLadeRimpactDO = this.ObjectMapper.Map<XsLadeRimpactDO>(dto);
            xsLadeRimpactDO.CreateBy = model.UserId;
            xsLadeRimpactDO.CreateByName = model.UserName;
            xsLadeRimpactDO.CreateDate = System.DateTime.Now;
            xsLadeRimpactDO.ModityBy = model.UserId;
            xsLadeRimpactDO.ModityByName = model.UserName;
            xsLadeRimpactDO.ModityDate = System.DateTime.Now;
            bool bRet = await _xsLadeRimpactRepository.AddAsync(xsLadeRimpactDO);
            if (!bRet) { return; }

            //更新服务状态
            XsLadeRimpactState xsLadeRimpactState = this.ObjectMapper.Map<XsLadeRimpactState>(xsLadeRimpactDO);

            xsLadeRimpactState.Transactions[transactionId] = true;//执行成功赋值

            await this.Persist(ProcessAction.Create, xsLadeRimpactState);
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="isDel"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task RevertAsync(Guid transactionId, int isDel, string userId, string userName)
        {
            if (State.Transactions.ContainsKey(transactionId))
            {
                if (State.Transactions[transactionId] == false)
                {
                    return;
                }

                XsLadeRimpactState xsLadeRimpactState = this.State;
                xsLadeRimpactState.IsDel = isDel;
                xsLadeRimpactState.ModityBy = userId;
                xsLadeRimpactState.ModityByName = userName;
                xsLadeRimpactState.ModityDate = System.DateTime.Now;
                xsLadeRimpactState.Version++;
                XsLadeRimpactDO xsLadeRimpactDO = this.ObjectMapper.Map<XsLadeRimpactDO>(xsLadeRimpactState);
                bool bRet = await _xsLadeRimpactRepository.SetAsync(xsLadeRimpactDO);
                if (!bRet) { return; }

                xsLadeRimpactState.Transactions[transactionId] = false;

                await this.Persist(ProcessAction.Update, xsLadeRimpactState);
            }
        }
    }
}
