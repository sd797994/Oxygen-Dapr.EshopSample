using IApplicationService.Base.AppQuery;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PermissionService
{
    [RemoteService("accountservice", "permissionquery", "权限服务")]
    public interface IPermissionQueryService
    {
        [RemoteFunc(funcDescription: "获取初始化权限接口")]
        Task<ApiResult> GetInitPermissionApilist();

        [RemoteFunc(funcDescription: "获取权限列表")]
        Task<ApiResult> GetPermissionList(PageQueryInputBase input);

        [RemoteFunc(funcDescription: "获取所有权限")]
        Task<ApiResult> GetAllPermissions();

        [RemoteFunc(funcDescription: "获取用户路由")]
        Task<ApiResult> GetUserRouter();
    }
}
