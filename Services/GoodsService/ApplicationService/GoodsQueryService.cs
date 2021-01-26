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

namespace ApplicationService
{
    public class GoodsQueryService : IApplicationService.GoodsService.GoodsQueryService
    {
        private readonly EfDbContext efDbContext;
        private readonly IStateManager stateManager;
        public GoodsQueryService(EfDbContext efDbContext, IStateManager stateManager)
        {
            this.efDbContext = efDbContext;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> GetGoodsList(PageQueryInputBase input)
        {
            var query = from goods in efDbContext.Goods.OrderBy(x => x.GoodsName)
                        join category in efDbContext.GoodsCategory on goods.CategoryId equals category.Id
                        select new GetGoodsListResponse() { Id = goods.Id, CategoryId = category.Id, CategoryName = category.CategoryName, GoodsImage = goods.GoodsImage, GoodsName = goods.GoodsName, ShelfState = goods.ShelfState, Stock = goods.Stock, Price = goods.Price };
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
    }
}
