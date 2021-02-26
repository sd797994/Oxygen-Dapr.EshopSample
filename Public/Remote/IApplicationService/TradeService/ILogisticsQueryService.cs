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
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> GetLogisticsList(PageQueryInputBase input);
    }
}
