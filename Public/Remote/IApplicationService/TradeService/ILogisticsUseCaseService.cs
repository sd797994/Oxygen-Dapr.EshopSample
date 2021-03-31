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
    [RemoteService("tradeservice", "logisticsusecase", "物流服务")]
    public interface ILogisticsUseCaseService
    {
        [RemoteFunc(funcDescription: "发货")]
        Task<ApiResult> Deliver(LogisticsDeliverDto input);
		
        [RemoteFunc(funcDescription: "确认收货")]
        Task<ApiResult> Receive(LogisticsReceiveDto input);
    }
}
