using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService
{
    [RemoteService("tradeservice", "logisticsquery", "物流服务")]
    public interface ILogisticsQueryService
    {
        [RemoteFunc(funcDescription: "获取物流信息")]
        Task<ApiResult> GetLogisticsList(PageQueryInputBase input);
    }
}
