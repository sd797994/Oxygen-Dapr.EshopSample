using Oxygen.Client.ServerSymbol;
using IApplicationService.TradeService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService
{
    [RemoteService("tradeservice", "orderusecase", "订单服务")]
    public interface IOrderUseCaseService
    {
        [RemoteFunc(funcDescription: "创建订单")]
        Task<ApiResult> CreateOrder(OrderCreateDto input);
    }
}
