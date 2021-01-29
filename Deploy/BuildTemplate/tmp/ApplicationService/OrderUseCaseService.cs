using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.PersistenceObject;
using IApplicationService.OrderService.Dtos.Output;
using IApplicationService.OrderService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;

namespace ApplicationService
{
    public class OrderUseCaseService : IApplicationService.OrderService.OrderUseCaseService
    {
        private readonly IOrderRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public OrderUseCaseService(IOrderRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> CreateOrder(OrderCreateDto input)
        {
            var entity = new Order();
            entity.CreateOrUpdate();
            repository.Add(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateOrder(OrderUpdateDto input)
        {
            var entity = await repository.GetAsync(input.Id);
            if (entity == null)
                throw new ApplicationServiceException();
            entity.CreateOrUpdate();
            repository.Update(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> DeleteOrder(OrderDeleteDto input)
        {
            var entity = await repository.GetAsync(input.Id);
            if (entity == null)
                throw new ApplicationServiceException();
            repository.Delete(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
    }
}
