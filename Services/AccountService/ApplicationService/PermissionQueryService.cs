using Domain.Repository;
using IApplicationService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class PermissionQueryService : IApplicationService.PermissionService.PermissionQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public static ConcurrentBag<AuthenticationInfo> PermissionApis { get; set; } = new ConcurrentBag<AuthenticationInfo>();
        public PermissionQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetInitPermissionApilist()
        {
            var result = PermissionApis.ToArray().ToList().Where(x => x.CheckPermission).GroupBy(x => x.Path).Select(y => new AuthenticationInfo(y.FirstOrDefault().SrvName, y.FirstOrDefault().FuncName, y.FirstOrDefault().CheckPermission, y.Key));
            return await ApiResult.Ok(result).Async();
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetPermissionList(PageQueryInputBase input)
        {
            var rootId = Guid.Empty;
            var query = from permission in dbContext.Permission
                        where permission.FatherId != rootId
                        join father in dbContext.Permission on permission.FatherId equals father.Id
                        select new GetPermissionListResponse()
                        {
                            Id = permission.Id,
                            ServerName = father.PermissionName,
                            PermissionName = permission.PermissionName,
                            Path = permission.Path
                        };
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            return ApiResult.Ok(new PageQueryResonseBase<GetPermissionListResponse>(Data, Total));
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAllPermissions()
        {
            var permissions = await dbContext.Permission.ToListAsync();
            var result = permissions.Where(x => x.FatherId == Guid.Empty).Select(x => new AllPermissionResponse()
            {
                Id = x.Id,
                PermissionName = x.PermissionName,
                Child = permissions.Where(y => y.FatherId == x.Id).Select(y => new AllPermissionResponse()
                {
                    Id = y.Id,
                    PermissionName = y.PermissionName
                })
            }).ToList();
            return ApiResult.Ok(result);
        }
    }
}
