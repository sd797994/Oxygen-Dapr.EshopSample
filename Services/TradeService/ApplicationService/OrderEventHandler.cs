using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using IApplicationService;
using IApplicationService.AppEvent;
using IApplicationService.TradeService.Dtos.Event;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Server.Kestrel.Implements;
using System;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class OrderEventHandler : IEventHandler
    {
        private readonly IOrderRepository repository;
        private readonly ITradeLogRepository tradeLogRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public OrderEventHandler(IOrderRepository repository, ITradeLogRepository tradeLogRepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.tradeLogRepository = tradeLogRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Order.ExpireCancelOrder)]
        public async Task<DefaultEventHandlerResponse> ExpireCancelOrder(EventHandleRequest<OperateOrderSuccDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(ExpireCancelOrder), input.GetDataJson(), async () =>
             {
                 var eventData = input.GetData();
                 var order = await repository.GetAsync(eventData.OrderId);
                 if (order == null)
                     throw new ApplicationServiceException($"没有找到订单");
                 if (order.CancelOrder())
                 {
                     repository.Update(order);
                     var tradeLog = new TradeLog();
                     tradeLog.CreateTradeLog(TradeLogState.CancelOrder, eventData.OrderId, order.OrderNo, null, null, Guid.Empty, null);
                     tradeLogRepository.Add(tradeLog);
                     await unitofWork.CommitAsync();
                 }
             });
        }
        [EventHandlerFunc(EventTopicDictionary.Order.CreateOrderSucc)]
        public async Task<DefaultEventHandlerResponse> RecordTradeLogByOrderCreate(EventHandleRequest<OperateOrderSuccDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(RecordTradeLogByOrderCreate), input.GetDataJson(), async () =>
            {
                var eventData = input.GetData();
                var order = await repository.GetAsync(eventData.OrderId);
                if (order == null)
                    throw new ApplicationServiceException($"没有找到订单");
                var tradeLog = new TradeLog();
                tradeLog.CreateTradeLog(TradeLogState.CreateOrder, eventData.OrderId, order.OrderNo, null, null, eventData.UserId, eventData.UserName);
                tradeLogRepository.Add(tradeLog);
                await unitofWork.CommitAsync();
            });
        }
        [EventHandlerFunc(EventTopicDictionary.Order.PayOrderSucc)]
        public async Task<DefaultEventHandlerResponse> RecordTradeLogByOrderPay(EventHandleRequest<OperateOrderSuccDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(RecordTradeLogByOrderPay), input.GetDataJson(), async () =>
            {
                var eventData = input.GetData();
                var order = await repository.GetAsync(eventData.OrderId);
                if (order == null)
                    throw new ApplicationServiceException($"没有找到订单");
                var tradeLog = new TradeLog();
                tradeLog.CreateTradeLog(TradeLogState.PayOrder, eventData.OrderId, order.OrderNo, null, null, eventData.UserId, eventData.UserName);
                tradeLogRepository.Add(tradeLog);
                await unitofWork.CommitAsync();
            });
        }
    }
}