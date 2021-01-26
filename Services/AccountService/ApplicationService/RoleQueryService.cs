using IApplicationService;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class RoleQueryService : IApplicationService.RoleService.RoleQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public RoleQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetRoleList(PageQueryInputBase input)
        {
            var query = from role in dbContext.Role
                        select new GetRoleListResponse()
                        {
                            RoleId = role.Id,
                            RoleName = role.RoleName,
                            SuperAdmin = role.SuperAdmin
                        };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            var roleIds = Data.Select(x => x.RoleId);
            var permissions = await (from rolepermission in dbContext.RolePermission
                                     where roleIds.Contains(rolepermission.RoleId)
                                     join permission in dbContext.Permission on rolepermission.PermissionId equals permission.Id
                                     select new { permission.Id, rolepermission.RoleId }).ToListAsync();
            Data.ForEach(x => x.Permissions = permissions.Where(y => y.RoleId == x.RoleId).Select(y => y.Id).ToList());
            return ApiResult.Ok(new PageQueryResonseBase<GetRoleListResponse>(Data, Total));
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAllRoles()
        {
            return await ApiResult.Ok(dbContext.Role.Select(x => new { RoleId = x.Id, RoleName = x.RoleName }).ToListAsync()).Async();
        }
    }
}
