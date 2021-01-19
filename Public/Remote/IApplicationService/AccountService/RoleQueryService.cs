using IApplicationService.Base.AppQuery;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.RoleService
{
    [RemoteService("accountservice", "rolequery", "角色服务")]
    public interface RoleQueryService
    {
        [RemoteFunc(funcDescription: "获取角色列表")]
        Task<ApiResult> GetRoleList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "获取所有角色")]
        Task<ApiResult> GetAllRoles();
    }
}
