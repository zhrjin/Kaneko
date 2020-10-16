using Kaneko.Core.ApiResult;
using Kaneko.Core.Contract;
using Kaneko.Server.Orleans.Grains;
using Kaneko.Server.Orleans.Services;
using Orleans;
using Orleans.Concurrency;
using Orleans.Sagas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.BasicData.IGrains.PbBasicFirmManager;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;
using YTSoft.BasicData.IGrains.XsCompyBaseManager;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;
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
    public class XsLadeBaseGrain : MainGrain, IXsLadeRimpactGrain
    {
        private readonly IXsLadeRimpactRepository _xsLadeRimpactRepository;
        private readonly IOrleansClient _orleansClient;

        public XsLadeBaseGrain(IXsLadeRimpactRepository scheduleTaskRepository, IOrleansClient orleansClient)
        {
            this._xsLadeRimpactRepository = scheduleTaskRepository;
            this._orleansClient = orleansClient;
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultPageLR<XsLadeRimpactVO>> GetPageLRSync(SearchDTO<XsLadeRimpactDTO> model)
        {
            var dto = model.Data;

            //根据发货单编码模糊查询获取所有发货单主键
            if (!string.IsNullOrEmpty(dto.XLB_LadeId))
            {
                var itemResult = await this.GrainFactory.GetGrain<IXsLadeBaseGrain>(this.GrainId).GetAllSync(new XsLadeBaseDTO { XLB_LadeId = dto.XLB_LadeId });
                if (itemResult.Success && itemResult.Data.Count > 0)
                {
                    dto.XLR_Lades = itemResult.Data.Where(m => !string.IsNullOrEmpty(m.Id)).Select(m => m.Id).Distinct().ToList();
                }
                else
                {
                    return ApiResultUtil.IsFailedPageLR<XsLadeRimpactVO>("无数据！");
                }
            }

            //根据查询条件获取分页列表数据
            var expression = dto.GetExpression();
            var orders = dto.GetOrder();
            var count = await _xsLadeRimpactRepository.CountAsync(expression);
            if (count == 0)
            {
                return ApiResultUtil.IsFailedPageLR<XsLadeRimpactVO>("无数据！");
            }
            var entities = await _xsLadeRimpactRepository.GetListAsync(model.PageIndex, model.PageSize, expression, isMaster: false, orderByFields: orders);

            //获取发货单数据
            List<XsLadeBaseDO> xsLadeBaseList = new List<XsLadeBaseDO>();
            if (entities.Any(m => !string.IsNullOrEmpty(m.XLR_Lade)))
            {
                var itemResult = await this.GrainFactory.GetGrain<IXsLadeBaseGrain>(this.GrainId).GetAllSync(new XsLadeBaseDTO
                {
                    Ids = entities.Where(m => !string.IsNullOrEmpty(m.XLR_Lade)).Select(m => m.XLR_Lade).Distinct().ToList()
                });
                if (itemResult.Success) { xsLadeBaseList = (List<XsLadeBaseDO>)itemResult.Data; }
            }

            //获取客商数据
            List<XsCompyBaseDO> xsCompyBaseList = new List<XsCompyBaseDO>();
            if (xsLadeBaseList != null)
            {
                var itemResult = await _orleansClient.GetGrain<IXsCompyBaseStateGrain>(GrainIdKey.UtcUIDGrainKey.ToString()).GetAllSync(new XsCompyBaseDTO
                {
                    Ids = xsLadeBaseList.Where(m => !string.IsNullOrEmpty(m.XLB_Client)).Select(m => m.XLB_Client).Distinct().ToList()
                });
                if (itemResult.Success) { xsCompyBaseList = (List<XsCompyBaseDO>)itemResult.Data; }
            }

            //获取企业数据
            List<PbBasicFirmDO> pbBasicFirmList = new List<PbBasicFirmDO>();
            if (entities.Any(m => !string.IsNullOrEmpty(m.XLR_Firm)))
            {
                var itemResult = await _orleansClient.GetGrain<IPbBasicFirmStateGrain>(GrainIdKey.UtcUIDGrainKey.ToString()).GetAllSync(new PbBasicFirmDTO
                {
                    Ids = entities.Where(m => !string.IsNullOrEmpty(m.XLR_Firm)).Select(m => m.XLR_Firm).Distinct().ToList()
                });
                if (itemResult.Success) { pbBasicFirmList = (List<PbBasicFirmDO>)itemResult.Data; }
            }

            //左连接查询数据
            var xsLadeRimpactVOs = (from rimpact in entities
                                    join t1 in xsLadeBaseList on rimpact.XLR_Lade equals t1.Id into mapping1
                                    from lade in mapping1.DefaultIfEmpty() //左连接需要加上DefaultIfEmpty
                                    join t2 in xsCompyBaseList on lade.XLB_Client equals t2.XOB_ID into mapping2
                                    from compy in mapping2.DefaultIfEmpty() //左连接需要加上DefaultIfEmpty
                                    join t3 in pbBasicFirmList on rimpact.XLR_Firm equals t3.PBF_ID into mapping3
                                    from firm in mapping3.DefaultIfEmpty() //左连接需要加上DefaultIfEmpty
                                    select new
                                    {
                                        rimpact,
                                        lade,
                                        compy,
                                        firm
                                    }).AsEnumerable()
                                    .Select(x =>
                                    {
                                        var result = this.ObjectMapper.Map<XsLadeRimpactVO>(x.rimpact);
                                        result.XLB_LadeId = x.lade?.XLB_LadeId;
                                        result.XLB_ClientName = x.compy?.XOB_Name;
                                        result.XLR_FirmName = x.firm?.PBF_ShortName;
                                        return result;
                                    })
                                    .ToList();

            //返回结果
            return ApiResultUtil.IsSuccess(xsLadeRimpactVOs, count, model.PageIndex, model.PageSize);
        }

        /// <summary>
        /// 新增发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddAsync(SubmitDTO<XsLadeRimpactDTO> model)
        {
            var dto = model.Data;

            var lbResult = await this.GrainFactory.GetGrain<IXsLadeBaseStateGrain>(dto.XLR_Lade).GetAsync();
            var lbData = lbResult.Success ? lbResult.Data : null;
            dto.XLR_Total = lbData?.XLB_FactTotal;

            //作废单新建
            var activitys = this.GrainFactory.CreateSaga().AddActivity<XsLadeRimpactActivity>(x =>
            {
                x.Add("GrainId", GrainId);
                x.Add("EventType", "ADD");
                x.Add("Data", model);
                 
            });

            //应收款
            var recResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
            {
                XRC_BillID = dto.XLR_Lade,
                XRC_Origins = { "102", "103", "104" }
            });
            var recData = recResult.Success ? recResult.Data : new List<XsReceReceivableDO>();

            switch (dto.XLR_Type)
            {
                case "101"://本月
                    //发货单状态更新
                    activitys.AddActivity<XsLadeBaseActivity>(x =>
                    {
                        x.Add("GrainId", dto.XLR_Lade);
                        x.Add("XLB_Status", "2");
                        x.Add("UserId", model.UserId);
                        x.Add("UserName", model.UserName);
                    });

                    foreach (var item in recData)
                    {
                        //应收款删除
                        activitys.AddActivity<XsReceReceivableActivity>(x =>
                        {
                            x.Add("GrainId", item.Id);
                            x.Add("EventType", "UPDATE");
                            x.Add("IsDel", 1);
                            x.Add("UserId", model.UserId);
                            x.Add("UserName", model.UserName);
                        });
                    }

                    break;

                case "102"://跨月
                    foreach (var item in recData)
                    {
                        string newId = await this.GrainFactory.GetGrain<IUtcUID>(GrainIdKey.UtcUIDGrainKey).NewID();

                        var submitData = new SubmitDTO<XsReceReceivableDTO>
                        {
                            Data = new XsReceReceivableDTO
                            {
                                Id = newId,
                                XRC_SetDate = dto.XLR_SetDate,
                                XRC_BillID = dto.Id,//作废主键
                                XRC_Origin = "4" + item.XRC_Origin,
                                XRC_Total = -item.XRC_Total,
                                XRC_Firm = dto.XLR_Firm
                            },
                            UserId = model.UserId,
                            UserName = model.UserName
                        };

                        //应收款新建
                        activitys.AddActivity<XsReceReceivableActivity>(x =>
                        {
                            x.Add("GrainId", newId);
                            x.Add("EventType", "ADD");
                            x.Add("Data", submitData);
                        });
                    }

                    break;
                default:
                    break;
            }

            //执行
            var saga = await activitys.ExecuteSagaAsync();
            await saga.Wait();

            if (SagaStatus.Executed == await saga.GetStatus())
            {
                return ApiResultUtil.IsSuccess("发货单作废成功新增成功！");
            }
            else
            {
                return ApiResultUtil.IsSuccess("发货单作废失败！");
            }
        }

        /// <summary>
        /// 删除发货单作废
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult> DelAsync(SubmitDTO<XsLadeRimpactDTO> model)
        {
            var dto = model.Data;

            //作废单删除
            var activitys = this.GrainFactory.CreateSaga().AddActivity<XsLadeRimpactActivity>(x =>
            {
                x.Add("GrainId", GrainId);
                x.Add("EventType", "UPDATE");
                x.Add("IsDel", 1);
                x.Add("UserId", model.UserId);
                x.Add("UserName", model.UserName);
            });

            //应收款
            List<string> XRCOrigins = "101" == dto.XLR_Type ? new List<string> { "102", "103", "104" } : new List<string> { "4102", "4103", "4104" };
            var recResult = await this.GrainFactory.GetGrain<IXsReceReceivableGrain>(this.GrainId).GetAllSync(new XsReceReceivableDTO
            {
                XRC_BillID = dto.XLR_Lade,
                XRC_Origins = XRCOrigins
            });
            var recData = recResult.Success ? recResult.Data : new List<XsReceReceivableDO>();

            switch (dto.XLR_Type)
            {
                case "101"://本月
                    //发货单状态更新
                    activitys.AddActivity<XsLadeBaseActivity>(x =>
                    {
                        x.Add("GrainId", dto.XLR_Lade);
                        x.Add("XLB_Status", "1");
                        x.Add("UserId", model.UserId);
                        x.Add("UserName", model.UserName);
                    });

                    foreach (var item in recData)
                    {
                        //还原应收款
                        activitys.AddActivity<XsReceReceivableActivity>(x =>
                        {
                            x.Add("GrainId", item.Id);
                            x.Add("EventType", "UPDATE");
                            x.Add("IsDel", 0);
                            x.Add("UserId", model.UserId);
                            x.Add("UserName", model.UserName);
                        });
                    }

                    break;

                case "102"://跨月
                    foreach (var item in recData)
                    {
                        //删除调整应收款
                        activitys.AddActivity<XsReceReceivableActivity>(x =>
                        {
                            x.Add("GrainId", item.Id);
                            x.Add("EventType", "UPDATE");
                            x.Add("IsDel", 1);
                            x.Add("UserId", model.UserId);
                            x.Add("UserName", model.UserName);
                        });
                    }

                    break;
                default:
                    break;
            }

            //执行
            var saga = await activitys.ExecuteSagaAsync();
            await saga.Wait();

            if (SagaStatus.Executed == await saga.GetStatus())
            {
                return ApiResultUtil.IsSuccess();
            }
            else
            {
                return ApiResultUtil.IsFailed("删除发货单作废失败！");
            }
        }
    }
}
