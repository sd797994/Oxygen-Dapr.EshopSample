using IApplicationService.AccountService.Dtos.Input;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.RoleService
{
    [RemoteService("accountservice", "roleusecase", "角色服务")]
    public interface IRoleUseCaseService
    {
        [RemoteFunc(funcDescription: "创建角色")]
        Task<ApiResult> RoleCreate(RoleCreateDto input);

        [RemoteFunc(funcDescription: "更新角色")]
        Task<ApiResult> RoleUpdate(RoleUpdateDto input);

        [RemoteFunc(funcDescription: "删除角色")]
        Task<ApiResult> RoleDelete(RoleDeleteDto input); 
    }
}
