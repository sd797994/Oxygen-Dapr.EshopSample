using Domain.Repository;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class %placeholder%UseCaseService : IApplicationService.%placeholder%Service.%placeholder%UseCaseService
    {
        private readonly I%placeholder%Repository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public %placeholder%UseCaseService(I%placeholder%Repository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}