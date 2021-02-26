using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.TradeService.Dtos.Input;

namespace IApplicationService.TradeService
{
    [RemoteService("tradeservice", "tradelogquery", "交易服务")]
    public interface ITradeLogQueryService
    {
        [RemoteFunc(funcDescription: "获取交易记录")]
        Task<ApiResult> GetTradeLogListByOrderId(GetTradeLogListByOrderIdDto input);
    }
}
