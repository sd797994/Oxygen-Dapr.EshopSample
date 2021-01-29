using Domain.Dtos;
using Domain.Repository;
using Domain.Services;
using IApplicationService;
using IApplicationService.AccountService.Dtos.Input;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureBase.Object;
using InfrastructureBase.AuthBase;
using IApplicationService.PermissionService;

namespace ApplicationService
{
    public class PermissionUseCaseService : IPermissionUseCaseService
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
        [AuthenticationFilter]
        public async Task<ApiResult> SavePermissions(List<CreatePermissionDto> input)
        {
            var result = input.CopyTo<CreatePermissionDto, CreatePermissionTmpDto>();
            new PermissionMultiCreateService(repository, result).Create();
            await unitofWork.CommitAsync();
            return await ApiResult.Ok("权限批量导入成功").Async();
        }
    }
}
