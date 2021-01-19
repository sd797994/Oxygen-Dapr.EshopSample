using Domain.Repository;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class UseCaseService : IApplicationService.GoodsService.GoodsUseCaseService
    {
        private readonly IGoodsRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public UseCaseService(IGoodsRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
