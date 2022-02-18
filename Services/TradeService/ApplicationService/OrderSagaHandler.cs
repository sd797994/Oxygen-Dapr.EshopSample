using Domain.Entities;
using Domain.Events;
using Domain.Repository;
using Domain.Services;
using Domain.Specification;
using Domain.ValueObject;
using IApplicationService.AccountService;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using IApplicationService.Sagas.CreateOrder.Dtos;
using IApplicationService.Sagas.CreateOrder.Handles;
using IApplicationService.TradeService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class OrderSagaHandler : IOrderHandle
    {
        private readonly IOrderRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IAccountQueryService accountQueryService;
        private readonly IGoodsQueryService goodsQueryService;
        private readonly IEventBus eventBus;
        public OrderSagaHandler(IOrderRepository repository, IUnitofWork unitofWork, IAccountQueryService accountQueryService, IEventBus eventBus, IGoodsQueryService goodsQueryService)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.accountQueryService = accountQueryService;
            this.eventBus = eventBus;
            this.goodsQueryService = goodsQueryService;
        }
        public async Task OrderCreate(OrderCreateDto dto)
        {
            try
            {
                var mockdata = await accountQueryService.GetMockAccount();
                var mockUser = mockdata.GetData<CurrentUser>();
                var createOrderService = new CreateOrderService(GetGoodsListByIds);
                var order = await createOrderService.FinalCreateOrder(mockUser.Id, mockUser.UserName, mockUser.Address, mockUser.Tel, dto.Items.CopyTo<OrderCreateDto.OrderCreateItemDto, OrderItem>().ToList());//通过订单服务创建订单
                repository.Add(order);
                if (await new CheckOrderCanCreateSpecification(repository).IsSatisfiedBy(order))
                    await unitofWork.CommitAsync();
                //发送订单创建成功事件(非saga主流程，用于作业系统定时取消未支付订单)
                await eventBus.SendEvent(EventTopicDictionary.Order.CreateOrderSucc, new OperateOrderSuccessEvent(order, mockUser.UserName));
            }
            catch(Exception e)
            {
                throw new SagaException<OrderCreateDto>(dto, e.Message);
            }
        }
        async Task<List<OrderGoodsSnapshot>> GetGoodsListByIds(IEnumerable<Guid> input)
        {
            return (await goodsQueryService.GetGoodsListByIds(new GetGoodsListByIdsDto(input))).GetData<List<OrderGoodsSnapshot>>();
        }
    }
}
