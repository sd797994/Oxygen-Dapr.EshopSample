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
            var query = from order in dbContext.Order.Where(x => x.OrderState != Domain.Enums.OrderState.Cancel && x.OrderState != Domain.Enums.OrderState.Create)
                        join logisticstmp in dbContext.Logistics.DefaultIfEmpty() on order.Id equals logisticstmp.OrderId into tmp
                        from logistics in tmp.DefaultIfEmpty()
                        orderby logistics.DeliveTime descending
                        select new GetLogisticsListResponse()
                        {
                            OrderId = order.Id,
                            Id = logistics == null ? null : logistics.Id,
                            OrderNo = order.OrderNo,
                            LogisticsType = logistics == null ? null : (int)logistics.LogisticsType,
                            LogisticsNo = logistics.LogisticsNo,
                            DeliverName = logistics.DeliverName,
                            DeliverAddress = logistics.DeliverAddress,
                            ReceiverName = logistics.ReceiverName,
                            ReceiverAddress = logistics.ReceiverAddress,
                            DeliveTime = logistics.DeliveTime,
                            ReceiveTime = logistics.ReceiveTime,
                            LogisticsState = logistics == null ? null : (int)logistics.LogisticsState,
                        };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetLogisticsListResponse>(Data, Total));
        }
    }
}
