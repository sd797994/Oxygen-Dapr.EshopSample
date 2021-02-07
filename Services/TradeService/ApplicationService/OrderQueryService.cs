using Domain;
using Domain.Repository;
using IApplicationService;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using IApplicationService.TradeService;
using System.Collections.Generic;
using Domain.Enums;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using System;

namespace ApplicationService
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IAccountQueryService accountQuery;
        private readonly IStateManager stateManager;
        public OrderQueryService(EfDbContext dbContext, IStateManager stateManager, IAccountQueryService accountQuery)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
            this.accountQuery = accountQuery;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetOrderList(PageQueryInputBase input)
        {
            var query = from order in dbContext.Order
                        select new
                        {
                            UserId = order.UserId,
                            OrderId = order.Id,
                            OrderNo = order.OrderNo,
                            OrderState = order.OrderState,
                            TotalPrice = order.TotalPrice,
                            CreateTime = order.CreateTime
                        };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query.OrderByDescending(x => x.CreateTime), input.Page, input.Limit);
            var orderIds = Data.Select(x => x.OrderId).ToList();
            var orderItems = dbContext.OrderItem.Where(x => orderIds.Contains(x.OrderId)).Select(item => new
            {
                OrderId = item.OrderId,
                GoodsName = item.GoodsSnapshot.GoodsName,
                Price = item.TotalPrice,
                Count = item.Count
            }).ToList();
            var usernames = (await accountQuery.GetAccountUserNameByIds(new GetAccountUserNameByIdsDto() { Ids = Data.Select(x => x.UserId).ToList() })).GetData<List<GetAccountUserNameByIdsResponse>>();
            var result = Data.Select(x => new GetOrderListResponse()
            {
                OrderId = x.OrderId,
                OrderNo = x.OrderNo,
                UserName = usernames.FirstOrDefault(y => x.UserId == y.AccountId).Name,
                OrderState = Convert.ToInt32(x.OrderState),
                TotalPrice = x.TotalPrice,
                OrderItems = orderItems.Where(y => y.OrderId == x.OrderId).Select(item => new GetOrderListItemResponse
                {
                    GoodsName = item.GoodsName,
                    Price = item.Price,
                    Count = item.Count
                }),
                CreateTime = x.CreateTime
            }).ToList();
            return ApiResult.Ok(new PageQueryResonseBase<GetOrderListResponse>(result, Total));
        }
    }
}
