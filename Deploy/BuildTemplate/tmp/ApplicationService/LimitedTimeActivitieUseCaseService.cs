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
using IApplicationService.LimitedTimeActivitieService.Dtos.Output;
using IApplicationService.LimitedTimeActivitieService.Dtos.Input;

namespace ApplicationService
{
    public class LimitedTimeActivitieUseCaseService : IApplicationService.LimitedTimeActivitieService.LimitedTimeActivitieUseCaseService
    {
        private readonly ILimitedTimeActivitieRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LimitedTimeActivitieUseCaseService(ILimitedTimeActivitieRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> CreateLimitedTimeActivitie(LimitedTimeActivitieCreateDto input)
        {
            var entity = new LimitedTimeActivitie();
            entity.CreateOrUpdate();
            repository.Add(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
		
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateLimitedTimeActivitie(LimitedTimeActivitieUpdateDto input)
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
        public async Task<ApiResult> DeleteLimitedTimeActivitie(LimitedTimeActivitieDeleteDto input)
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
