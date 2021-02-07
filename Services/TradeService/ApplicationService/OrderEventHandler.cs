using Domain.Repository;
using IApplicationService.AppEvent;
using IApplicationService.TradeService.Dtos.Event;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class OrderEventHandler : IEventHandler
    {
        private readonly IOrderRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public OrderEventHandler(IOrderRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Order.ExpireCancelOrder)]
        public async Task<DefaultEventHandlerResponse> LoginCacheExpireJob(EventHandleRequest<CreateOrderSuccDto> input)
        {
            var order = await repository.GetAsync(input.data.OrderId);
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
        [EventHandlerFunc(EventTopicDictionary.Order.CreateOrderSucc)]
        public async Task<DefaultEventHandlerResponse> RecordTradeLogByOrderCreate(EventHandleRequest<CreateOrderSuccDto> input)
        {
            Console.WriteLine($"{DateTime.Now}系统创建了一条交易流水");
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
    }
}