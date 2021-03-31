using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using IApplicationService.TradeService;
using IApplicationService.GoodsService;
using Domain.Services;
using DomainBase;
using System.Collections.Generic;
using Domain.Entities;
using InfrastructureBase.Object;
using Domain.Specification;
using Domain.ValueObject;
using Domain.Dtos;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase.Http;
using IApplicationService.AppEvent;
using Domain.Events;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Output;

namespace ApplicationService
{
    public class OrderUseCaseService : IOrderUseCaseService
    {
        private readonly IOrderRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IGoodsQueryService goodsQueryService;
        private readonly IGoodsActorService goodsActorService;
        private readonly IAccountQueryService accountQueryService;
        public OrderUseCaseService(IOrderRepository repository, IAccountQueryService accountQueryService, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork, IGoodsQueryService goodsQueryService, IGoodsActorService goodsActorService)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.goodsQueryService = goodsQueryService;
            this.goodsActorService = goodsActorService;
            this.accountQueryService = accountQueryService;
        }

        public async Task<ApiResult> CreateOrder(OrderCreateDto input)
        {
            var mockUser = (await accountQueryService.GetMockAccount()).GetData<CurrentUser>();
            //申明一个创建订单领域服务实例,将远程rpc调用作为匿名函数传递进去
            var createOrderService = new CreateOrderService(GetGoodsListByIds, DeductionGoodsStock, UnDeductionGoodsStock);
            return await ApiResult.Ok("订单创建成功!").RunAsync(async () =>
            {
                var order = await createOrderService.CreateOrder(mockUser.Id, mockUser.UserName, mockUser.Address, mockUser.Tel, input.Items.CopyTo<OrderCreateDto.OrderCreateItemDto, OrderItem>().ToList());//通过订单服务创建订单
                repository.Add(order);
                if (await new CheckOrderCanCreateSpecification(repository).IsSatisfiedBy(order))
                    await unitofWork.CommitAsync();
                await eventBus.SendEvent(EventTopicDictionary.Order.CreateOrderSucc, new OperateOrderSuccessEvent(order, mockUser.UserName));//发送订单创建成功事件
            },
            //失败回滚
            createOrderService.UnCreateOrder);
        }
        public async Task<ApiResult> OrderPay(OrderPayDto input)
        {
            var mockUser = (await accountQueryService.GetMockAccount()).GetData<CurrentUser>();
            var order = await repository.GetAsync(input.OrderId);
            if (order == null)
                throw new ApplicationServiceException("没有找到该订单!");
            order.PayOrder(mockUser.Id);
            repository.Update(order);
            await unitofWork.CommitAsync();
            await eventBus.SendEvent(EventTopicDictionary.Order.PayOrderSucc, new OperateOrderSuccessEvent(order, mockUser.UserName));//发送订单支付成功事件
            return ApiResult.Ok();
        }

        #region 私有远程服务包装器方法
        async Task<List<OrderGoodsSnapshot>> GetGoodsListByIds(IEnumerable<Guid> input)
        {
            return (await goodsQueryService.GetGoodsListByIds(new GetGoodsListByIdsDto(input))).GetData<List<OrderGoodsSnapshot>>();
        }
        async Task<bool> DeductionGoodsStock(CreateOrderDeductionGoodsStockDto input)
        {
            var data = input.CopyTo<CreateOrderDeductionGoodsStockDto, DeductionStockDto>();
            data.ActorId = input.GoodsId.ToString();
            return (await goodsActorService.DeductionGoodsStock(data)).GetData<bool>();
        }
        async Task<bool> UnDeductionGoodsStock(CreateOrderDeductionGoodsStockDto input)
        {
            var data = input.CopyTo<CreateOrderDeductionGoodsStockDto, DeductionStockDto>();
            data.ActorId = input.GoodsId.ToString();
            return (await goodsActorService.UnDeductionGoodsStock(data)).GetData<bool>();
        }
        #endregion
    }
}
