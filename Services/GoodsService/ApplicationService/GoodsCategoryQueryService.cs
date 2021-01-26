using IApplicationService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsCategoryQueryService : IApplicationService.GoodsCategoryService.GoodsCategoryQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public GoodsCategoryQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }

        public async Task<ApiResult> GetAllCategoryList()
        {
            var query = from category in dbContext.GoodsCategory orderby category.Sort select new { category.Id, category.CategoryName };
            return await ApiResult.Ok(query).Async();
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetCategoryList(PageQueryInputBase input)
        {
            var query = from category in dbContext.GoodsCategory orderby category.Sort select category;
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GoodsCategory>(Data, Total));
        }
    }
}
