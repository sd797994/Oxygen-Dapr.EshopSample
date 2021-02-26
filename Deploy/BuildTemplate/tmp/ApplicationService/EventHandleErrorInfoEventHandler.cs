using Domain.Repository;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;

namespace ApplicationService
{
    public class EventHandleErrorInfoEventHandler : IEventHandler
    {
        private readonly IEventHandleErrorInfoRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public EventHandleErrorInfoEventHandler(IEventHandleErrorInfoRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
    }
}
