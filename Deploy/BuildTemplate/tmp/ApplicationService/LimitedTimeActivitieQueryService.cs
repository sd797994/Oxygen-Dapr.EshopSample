using Domain;
using Domain.Repository;
using IApplicationService;
using IApplicationService.LimitedTimeActivitieService.Dtos.Output;
using IApplicationService.LimitedTimeActivitieService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class LimitedTimeActivitieQueryService : IApplicationService.LimitedTimeActivitieService.LimitedTimeActivitieQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public LimitedTimeActivitieQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetLimitedTimeActivitieList(PageQueryInputBase input)
        {
            var query = from LimitedTimeActivitie in dbContext.LimitedTimeActivitie select new GetLimitedTimeActivitieListResponse() { };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetLimitedTimeActivitieListResponse>(Data, Total));
        }
		
    }
}
