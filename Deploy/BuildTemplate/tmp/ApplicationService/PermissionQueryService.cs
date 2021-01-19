using Domain.Repository;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class PermissionQueryService : IApplicationService.PermissionService.PermissionQueryService
    {
        private readonly IPermissionRepository repository;
        private readonly IStateManager stateManager;
        public PermissionQueryService(IPermissionRepository repository, IStateManager stateManager)
        {
            this.repository = repository;
            this.stateManager = stateManager;
        }
    }
}
