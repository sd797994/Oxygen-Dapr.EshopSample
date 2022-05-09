using IApplicationService;
using IApplicationService.Base.AppQuery;
using IApplicationService.GoodsCategoryService;
using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsCategoryQueryService : IGoodsCategoryQueryService
    {
        private readonly EfDbContext dbContext;
        public GoodsCategoryQueryService(EfDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ApiResult> GetAllCategoryList()
        {
            var query = from category in dbContext.GoodsCategory orderby category.Sort select new { category.Id, category.CategoryName };
            return await ApiResult.Ok(query.ToListAsync()).Async();
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
