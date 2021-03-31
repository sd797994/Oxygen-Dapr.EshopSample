using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Domain.Services;
using IApplicationService;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
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
    public class TradeEventHandler : IEventHandler
    {
        private readonly IOrderRepository repository;
        private readonly ILogisticsRepository logisticsRepository;
        private readonly ITradeLogRepository tradeLogRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IGoodsActorService goodsActorService;
        public TradeEventHandler(IOrderRepository repository, ILogisticsRepository logisticsRepository, ITradeLogRepository tradeLogRepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork, IGoodsActorService goodsActorService)
        {
            this.repository = repository;
            this.logisticsRepository = logisticsRepository;
            this.tradeLogRepository = tradeLogRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.goodsActorService = goodsActorService;
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
                 var cancelOrderService = new CancelOrderService(UnDeductionGoodsStock);
                 if (await cancelOrderService.Cancel(order))
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
        [EventHandlerFunc(EventTopicDictionary.Logistics.LogisticsDeliverSucc)]
        public async Task<DefaultEventHandlerResponse> RecordTradeLogByLogisticsDeliverSucc(EventHandleRequest<CreateLogisticsSuccDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(RecordTradeLogByOrderPay), input.GetDataJson(), async () =>
            {
                var eventData = input.GetData();
                var order = await repository.GetAsync(eventData.OrderId);
                if (order == null)
                    throw new ApplicationServiceException($"没有找到订单");
                var log = await logisticsRepository.GetAsync(eventData.LogisticsId);
                if (log == null)
                    throw new ApplicationServiceException($"没有找到物流单");
                var tradeLog = new TradeLog();
                tradeLog.CreateTradeLog(TradeLogState.DeliverGoods, eventData.OrderId, order.OrderNo, log.Id, log.LogisticsNo, eventData.UserId, eventData.UserName);
                tradeLogRepository.Add(tradeLog);
                await unitofWork.CommitAsync();
            });
        }
        [EventHandlerFunc(EventTopicDictionary.Logistics.LogisticsReceiveSucc)]
        public async Task<DefaultEventHandlerResponse> RecordTradeLogByLogisticsReceiveSucc(EventHandleRequest<CreateLogisticsSuccDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(RecordTradeLogByOrderPay), input.GetDataJson(), async () =>
            {
                var eventData = input.GetData();
                var order = await repository.GetAsync(eventData.OrderId);
                if (order == null)
                    throw new ApplicationServiceException($"没有找到订单");
                var log = await logisticsRepository.GetAsync(eventData.LogisticsId);
                if (log == null)
                    throw new ApplicationServiceException($"没有找到物流单");
                var tradeLog = new TradeLog();
                tradeLog.CreateTradeLog(TradeLogState.ReceivingGoods, eventData.OrderId, order.OrderNo, log.Id, log.LogisticsNo, eventData.UserId, eventData.UserName);
                tradeLogRepository.Add(tradeLog);
                await unitofWork.CommitAsync();
            });
        }
        #region 私有方法
        async Task<bool> UnDeductionGoodsStock(CreateOrderDeductionGoodsStockDto input)
        {
            var data = input.CopyTo<CreateOrderDeductionGoodsStockDto, DeductionStockDto>();
            data.ActorId = input.GoodsId.ToString();
            return (await goodsActorService.UnDeductionGoodsStock(data)).GetData<bool>();
        }
        #endregion
    }
}
