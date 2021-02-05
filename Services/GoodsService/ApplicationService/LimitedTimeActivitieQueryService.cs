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
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using System;
using IApplicationService.LimitedTimeActivitieService;

namespace ApplicationService
{
    public class LimitedTimeActivitieQueryService : ILimitedTimeActivitieQueryService
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
            var query = from activitie in dbContext.LimitedTimeActivitie
                        join goods in dbContext.Goods on activitie.GoodsId equals goods.Id
                        select new GetLimitedTimeActivitieListResponse()
                        {
                            Id = activitie.Id,
                            GoodsId = activitie.GoodsId,
                            ActivitieName = activitie.ActivitieName,
                            GoodsName = goods.GoodsName,
                            Price = goods.Price,
                            ActivitiePrice = activitie.ActivitiePrice,
                            StartTime = activitie.StartTime,
                            EndTime = activitie.EndTime,
                            ShelfState = activitie.ShelfState,
                        };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query.OrderByDescending(x => x.EndTime), input.Page, input.Limit);
            Data.ForEach(x =>
            {
                x.ActivitieState = DateTime.Now < x.StartTime ? 0 : x.StartTime < DateTime.Now && x.EndTime > DateTime.Now ? 1 : 2;
                x.ActivitieStateInfo = "";
            });
            return ApiResult.Ok(new PageQueryResonseBase<GetLimitedTimeActivitieListResponse>(Data, Total));
        }
		
    }
}
