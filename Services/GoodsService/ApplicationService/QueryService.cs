using Domain.Repository;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class QueryService : IApplicationService.GoodsService.GoodsQueryService
    {
        private readonly IGoodsRepository repository;
        private readonly IStateManager stateManager;
        public QueryService(IGoodsRepository repository, IStateManager stateManager)
        {
            this.repository = repository;
            this.stateManager = stateManager;
        }
    }
}
