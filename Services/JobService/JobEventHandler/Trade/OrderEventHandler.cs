using Hangfire;
using IApplicationService.AppEvent;
using IApplicationService.TradeService.Dtos.Event;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.JobEventHandler.Trade
{
    public class OrderEventHandler : IEventHandler
    {
        [EventHandlerFunc(EventTopicDictionary.Order.CreateOrderSucc)]
        public async Task<DefaultEventHandlerResponse> CancelOrderJob(EventHandleRequest<CreateOrderSuccDto> input)
        {
            var jobid = BackgroundJob.Schedule<IEventBus>(x => x.SendEvent(EventTopicDictionary.Order.ExpireCancelOrder, input.data), TimeSpan.FromMinutes(1));
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
    }
}