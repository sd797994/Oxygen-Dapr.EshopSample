using Domain;
using Domain.Repository;
using IApplicationService;
using IApplicationService.OrderService.Dtos.Output;
using IApplicationService.OrderService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;

namespace ApplicationService
{
    public class OrderQueryService : IApplicationService.OrderService.OrderQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public OrderQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetOrderList(PageQueryInputBase input)
        {
            var query = from Order in dbContext.Order select new GetOrderListResponse() { };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetOrderListResponse>(Data, Total));
        }
		
    }
}
