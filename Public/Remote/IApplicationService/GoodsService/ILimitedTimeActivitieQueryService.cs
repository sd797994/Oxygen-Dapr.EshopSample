using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.LimitedTimeActivitieService
{
    [RemoteService("goodsservice", "activitiquery", "限时活动服务")]
    public interface ILimitedTimeActivitieQueryService
    {
        [RemoteFunc(funcDescription: "获取限时活动列表")]
        Task<ApiResult> GetLimitedTimeActivitieList(PageQueryInputBase input);
    }
}
