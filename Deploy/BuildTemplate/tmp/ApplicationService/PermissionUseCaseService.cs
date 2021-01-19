using Domain.Repository;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class PermissionUseCaseService : IApplicationService.PermissionService.PermissionUseCaseService
    {
        private readonly IPermissionRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public PermissionUseCaseService(IPermissionRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
