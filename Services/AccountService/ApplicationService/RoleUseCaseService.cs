using Domain;
using Domain.Repository;
using Domain.Specification;
using IApplicationService;
using IApplicationService.AccountService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class RoleUseCaseService : IApplicationService.RoleService.RoleUseCaseService
    {
        private readonly IRoleRepository rolerepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public RoleUseCaseService(IRoleRepository rolerepository, IPermissionRepository permissionRepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.rolerepository = rolerepository;
            this.permissionRepository = permissionRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> RoleCreate(RoleCreateDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var role = new Role();
            role.SetRole(input.RoleName, input.SuperAdmin, input.Permissions);
            await rolerepository.Add(role);
            if (await new PermissionValidityCheckSpecification(permissionRepository).IsSatisfiedBy(role))
                await unitofWork.CommitAsync(tran);
            return ApiResult.Ok();
        }
        [AuthenticationFilter]
        public async Task<ApiResult> RoleUpdate(RoleUpdateDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var role = await rolerepository.GetAsync(input.RoleId);
            role.SetRole(input.RoleName, input.SuperAdmin, input.Permissions);
            await rolerepository.Update(role);
            if (await new PermissionValidityCheckSpecification(permissionRepository).IsSatisfiedBy(role))
                await unitofWork.CommitAsync(tran);
            return ApiResult.Ok();
        }
    }
}
