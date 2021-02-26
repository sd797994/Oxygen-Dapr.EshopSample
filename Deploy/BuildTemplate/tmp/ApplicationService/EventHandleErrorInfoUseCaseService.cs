using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.PublicService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.PersistenceObject;
using IApplicationService.PublicService.Dtos.Output;
using IApplicationService.PublicService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using Domain.Entities;

namespace ApplicationService
{
    public class EventHandleErrorInfoUseCaseService : IEventHandleErrorInfoUseCaseService
    {
        private readonly IEventHandleErrorInfoRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public EventHandleErrorInfoUseCaseService(IEventHandleErrorInfoRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> CreateEventHandleErrorInfo(EventHandleErrorInfoCreateDto input)
        {
            var entity = new EventHandleErrorInfo();
            entity.CreateOrUpdate();
            repository.Add(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateEventHandleErrorInfo(EventHandleErrorInfoUpdateDto input)
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
        public async Task<ApiResult> DeleteEventHandleErrorInfo(EventHandleErrorInfoDeleteDto input)
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
