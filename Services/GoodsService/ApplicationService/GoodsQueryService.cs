using Domain.Repository;
using IApplicationService;
using IApplicationService.Base.AppQuery;
using IApplicationService.GoodsService.Dtos.Output;
using Infrastructure.EfDataAccess;
using InfrastructureBase.Data;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using System.Linq;
using IApplicationService.GoodsService.Dtos.Input;
using InfrastructureBase.Object;
using InfrastructureBase.AuthBase;
using IApplicationService.GoodsService;
using System;
using Microsoft.EntityFrameworkCore;
using Autofac;

namespace ApplicationService
{
    public class GoodsQueryService : IGoodsQueryService
    {
        private readonly EfDbContext efDbContext;
        private readonly IStateManager stateManager;
        private readonly ILifetimeScope lifetimeScope;
        public GoodsQueryService(EfDbContext efDbContext, IStateManager stateManager,ILifetimeScope lifetimeScope)
        {
            this.efDbContext = efDbContext;
            this.stateManager = stateManager;
            this.lifetimeScope = lifetimeScope;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> GetGoodsList(PageQueryInputBase input)
        {
            var query = (from goods in efDbContext.Goods.OrderBy(x => x.GoodsName)
                         join category in efDbContext.GoodsCategory on goods.CategoryId equals category.Id
                         select new GetGoodsListResponse() { Id = goods.Id, CategoryId = category.Id, CategoryName = category.CategoryName, GoodsImage = goods.GoodsImage, GoodsName = goods.GoodsName, ShelfState = goods.ShelfState, Stock = goods.Stock, Price = goods.Price }).OrderBy(x => x.GoodsName);
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetGoodsListResponse>(Data, Total));
        }
        //todo: 面向前端改成基于elasticsearch
        public async Task<ApiResult> GetGoodslistByGoodsName(GetGoodslistByGoodsNameDto input)
        {
            if (string.IsNullOrEmpty(input.GoodsName))
                return ApiResult.Ok();
            else
            {
                var query = from goods in efDbContext.Goods.Where(x => x.GoodsName.Contains(input.GoodsName)).OrderBy(x => x.GoodsName).Take(10) select new { Id = goods.Id, GoodsName = goods.GoodsName, Price = goods.Price };
                return await ApiResult.Ok(query).Async();
            }
        }

        public async Task<ApiResult> GetGoodsListByIds(GetGoodsListByIdsDto input)
        {
            var now = DateTime.Now;
            var query = from goods in efDbContext.Goods.Where(x => input.Ids.Contains(x.Id) && x.ShelfState == true)
                        join category in efDbContext.GoodsCategory on goods.CategoryId equals category.Id
                        join activitie1 in efDbContext.LimitedTimeActivitie.Where(x => x.ShelfState && x.StartTime <= now && x.EndTime >= now).DefaultIfEmpty() on goods.Id equals activitie1.GoodsId into tmp1
                        from activitie in tmp1.DefaultIfEmpty()
                        select new { GoodsId = goods.Id, CategoryName = category.CategoryName, GoodsName = goods.GoodsName, GoodsImage = goods.GoodsImage, OriginalPrice = goods.Price, Price = activitie == null ? goods.Price : activitie.ActivitiePrice };
            return await ApiResult.Ok(query.ToListAsync()).Async();
        }
    }
}
