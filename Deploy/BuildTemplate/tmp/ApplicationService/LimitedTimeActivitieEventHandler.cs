using Domain.Repository;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;

namespace ApplicationService
{
    public class LimitedTimeActivitieEventHandler : IEventHandler
    {
        private readonly ILimitedTimeActivitieRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LimitedTimeActivitieEventHandler(ILimitedTimeActivitieRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
