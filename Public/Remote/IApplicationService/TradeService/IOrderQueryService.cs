using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.TradeService.Dtos.Input;

namespace IApplicationService.TradeService
{
    [RemoteService("tradeservice", "orderquery", "订单服务")]
    public interface IOrderQueryService
    {
        [RemoteFunc(funcDescription: "获取订单列表")]
        Task<ApiResult> GetOrderList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "获取订单售卖数")]
        Task<ApiResult> GetOrderSellCountByGoodsId(GetOrderSellCountByGoodsIdDto input);
    }
}
