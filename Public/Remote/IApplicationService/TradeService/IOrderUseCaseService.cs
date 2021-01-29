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
    [RemoteService("", "", "")]
    public interface IOrderUseCaseService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> CreateOrder(OrderCreateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> UpdateOrder(OrderUpdateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> DeleteOrder(OrderDeleteDto input);
    }
}
