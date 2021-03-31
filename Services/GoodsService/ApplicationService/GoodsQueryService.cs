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
using InfrastructureBase.Data.Nest;
using Infrastructure.PersistenceObject;
using Infrastructure.Elasticsearch;
using System.Collections.Generic;

namespace ApplicationService
{
    public class GoodsQueryService : IGoodsQueryService
    {
        private readonly EfDbContext efDbContext;
        private readonly IStateManager stateManager;
        private readonly IElasticSearchRepository<EsGoodsDto> searchRepository;
        public GoodsQueryService(EfDbContext efDbContext, IStateManager stateManager, ILifetimeScope lifetimeScope, IElasticSearchRepository<EsGoodsDto> searchRepository)
        {
            this.efDbContext = efDbContext;
            this.stateManager = stateManager;
            this.searchRepository = searchRepository;
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

        public async Task<ApiResult> GetGoodslistByGoodsName(GetGoodslistByGoodsNameDto input)
        {
            if (string.IsNullOrEmpty(input.GoodsName))
                return ApiResult.Ok();
            else
            {
                var result = (await searchRepository.GetRepo("goods").Query(x => x.Name.Contains(input.GoodsName)).Sort(x => x.Name, false).Page(input.Page, input.Limit).SearchData()).Select(goods => new { Id = goods.Id, GoodsName = goods.Name, Price = goods.Price });
                return await ApiResult.Ok(result).Async();
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

        public async Task<ApiResult> GetEsGoods(PageQueryInputBase input)
        {
            //查询所有的上架商品id
            var ids = await efDbContext.Goods.Where(x => x.ShelfState == true).Select(x => x.Id).ToListAsync();
            //带入es查询详情
            if (ids.Any())
            {
                var goods = await searchRepository.GetRepo("goods").Sort(x => x.Name, false).Page(ids.Count, 1).Query(x => ids.Contains(x.Id)).SearchData();
                if (goods.Any())
                {
                    var result = goods.GroupBy(x => x.CategoryId).Select(x => new
                    {
                        name = x.FirstOrDefault().CategoryName,
                        type = 0,
                        foods = x.Select(y => new
                        {
                            id = y.Id,
                            description = y.CategoryName,
                            icon = y.Icon,
                            image = y.Icon,
                            info = y.Description,
                            name = y.Name,
                            oldPrice = y.OldPrice,
                            price = y.Price,
                            rating = 100,
                            ratings = new string[] { },
                            sellCount = y.SellCount
                        })
                    }).ToList();
                    return ApiResult.Ok(result);
                }
            }
            return ApiResult.Ok();
        }
    }
}
