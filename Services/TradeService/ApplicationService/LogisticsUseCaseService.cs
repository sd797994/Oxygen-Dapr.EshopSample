using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.TradeService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.PersistenceObject;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using Domain.Entities;
using InfrastructureBase.Object;

namespace ApplicationService
{
    public class LogisticsUseCaseService : ILogisticsUseCaseService
    {
        private readonly ILogisticsRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LogisticsUseCaseService(ILogisticsRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> Deliver(LogisticsDeliverDto input)
        {
            return await ApiResult.Ok().Async();
        }

        [AuthenticationFilter]
        public async Task<ApiResult> Receive(LogisticsReceiveDto input)
        {
            return await ApiResult.Ok().Async();
        }
    }
}
