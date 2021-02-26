using Domain.Repository;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Server.Kestrel.Implements;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class PermissionEventHandler : IEventHandler
    {
        private readonly IPermissionRepository repository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public PermissionEventHandler(IPermissionRepository repository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Common.InitAuthApiList)]
        public async Task<DefaultEventHandlerResponse> GetAllPermission(EventHandleRequest<InitPermissionApiEvent<List<AuthenticationInfo>>> input)
        {
            if (input.GetData().ServiceApis != null)
            {
                await PermissionListCacheHelper.GroupAndSave(stateManager, input.GetData().ServiceApis);
            }
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
    }
}
