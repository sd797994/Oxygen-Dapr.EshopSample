using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.TradeService;
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
using Microsoft.EntityFrameworkCore;

namespace ApplicationService
{
    public class LogisticsQueryService : ILogisticsQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public LogisticsQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetLogisticsList(PageQueryInputBase input)
        {
            var query = from Logistics in dbContext.Logistics select new GetLogisticsListResponse() { };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetLogisticsListResponse>(Data, Total));
        }
		
    }
}
