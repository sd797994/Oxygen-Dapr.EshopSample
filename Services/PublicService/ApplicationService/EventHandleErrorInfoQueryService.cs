using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.PublicService;
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
            var query = from eventHandleErrorInfo in dbContext.EventHandleErrorInfo.OrderByDescending(x=>x.LogDate) select eventHandleErrorInfo;
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<Infrastructure.PersistenceObject.EventHandleErrorInfo>(Data, Total));
        }
    }
}
