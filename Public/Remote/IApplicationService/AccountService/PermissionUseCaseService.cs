using IApplicationService.AccountService.Dtos.Input;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PermissionService
{
    [RemoteService("accountservice", "permissionusecase", "权限服务")]
    public interface PermissionUseCaseService
    {
        [RemoteFunc(funcDescription:"批量保存权限")]
        Task<ApiResult> SavePermissions(List<CreatePermissionDto> input);
    }
}
