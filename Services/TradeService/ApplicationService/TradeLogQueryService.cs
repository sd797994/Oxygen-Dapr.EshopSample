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
    public class TradeLogQueryService : ITradeLogQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public TradeLogQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetTradeLogListByOrderId(GetTradeLogListByOrderIdDto input)
        {
            var query = from TradeLog in dbContext.TradeLog.Where(x => x.OrderId == input.OrderId).OrderByDescending(x => x.TradeDate)
                        select new
                        {
                            TradeLog.TradeDate,
                            TradeLog.TradeLogValue
                        };
            return await ApiResult.Ok(query.ToListAsync()).Async();
        }
    }
}
