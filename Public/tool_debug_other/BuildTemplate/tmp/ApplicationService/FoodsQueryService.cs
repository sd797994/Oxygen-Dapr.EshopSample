using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Output;
using IApplicationService.GoodsService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService
{
    public class FoodsQueryService : IFoodsQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public FoodsQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetFoodsList(PageQueryInputBase input)
        {
            var query = from Foods in dbContext.Foods select new GetFoodsListResponse() { };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetFoodsListResponse>(Data, Total));
        }
		
    }
}
