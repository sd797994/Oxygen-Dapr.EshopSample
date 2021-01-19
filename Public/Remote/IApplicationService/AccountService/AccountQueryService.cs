using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.Base.AppQuery;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService
{
    [RemoteService("accountservice", "accountquery", "用户服务")]
    public interface AccountQueryService
    {
        [RemoteFunc(funcDescription: "获取用户信息")]
        Task<ApiResult> GetAccountInfo();

        [RemoteFunc(funcDescription: "检查是否初始化RBAC")]
        Task<ApiResult> CheckRoleBasedAccessControler();

        [RemoteFunc(funcDescription: "获取用户信息")]
        Task<ApiResult> GetAccountList(PageQueryInputBase input);
    }
}
