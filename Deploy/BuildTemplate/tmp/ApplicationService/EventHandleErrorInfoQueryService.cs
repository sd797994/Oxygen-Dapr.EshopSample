using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.PublicService;
using IApplicationService.PublicService.Dtos.Output;
using IApplicationService.PublicService.Dtos.Input;
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
    public class EventHandleErrorInfoQueryService : IEventHandleErrorInfoQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public EventHandleErrorInfoQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> GetEventHandleErrorInfoList(PageQueryInputBase input)
        {
            var query = from EventHandleErrorInfo in dbContext.EventHandleErrorInfo select new GetEventHandleErrorInfoListResponse() { };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetEventHandleErrorInfoListResponse>(Data, Total));
        }
		
    }
}
