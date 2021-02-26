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
using IApplicationService.PublicService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using Domain.Entities;
using MallSetting = Domain.Entities.MallSetting;

namespace ApplicationService
{
    public class MallSettingUseCaseService : IMallSettingUseCaseService
    {
        private readonly IMallSettingRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public MallSettingUseCaseService(IMallSettingRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> CreateOrUpdateMallSetting(CreateOrUpdateMallSettingDto input)
        {
            var entity = await repository.GetAsync() ?? new MallSetting();
            entity.CreateOrUpdate(input.DeliverName, input.DeliverAddress);
            if (await repository.AnyAsync())
                repository.Update(entity);
            else
                repository.Add(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok();
        }
    }
}
