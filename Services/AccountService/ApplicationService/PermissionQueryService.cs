using Domain.Repository;
using IApplicationService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
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

        [AuthenticationFilter(false)]
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
        /// <summary>
        /// 一个简易的模拟动态路由
        /// </summary>
        /// <returns></returns>
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetUserRouter()
        {
            var result = new List<ExpandoObject>();
            if (!HttpContextExt.Current.User.IgnorePermission)
            {
                void CheckPermission(string name,int father)
                {
                    if (!HttpContextExt.Current.User.Permissions.Any(x => x.Contains($"{name}query")) && !HttpContextExt.Current.User.Permissions.Any(x => x.Contains($"{name}usecase")))
                    {
                        father++;
                        dynamic item = new ExpandoObject();
                        item.path = $"{name}";
                        item.hidden = true;
                        result.Add(item);
                    }
                }
                int fatheraccount = 0;
                CheckPermission("account", fatheraccount);
                CheckPermission("permission", fatheraccount);
                CheckPermission("role", fatheraccount);
                if (fatheraccount >= 3)
                {
                    dynamic item = new ExpandoObject();
                    item.path = "/rbac";
                    item.hidden = true;
                    result.Add(item);
                }
                //if (!HttpContextExt.Current.User.Permissions.Any(x => x.Contains("goods")))
                //{
                //    dynamic item = new ExpandoObject();
                //    item.path = "goods";
                //    item.hidden = true;
                //    result.Add(item);
                //}
                //if (!HttpContextExt.Current.User.Permissions.Any(x => x.Contains("role")))
                //{
                //    dynamic item = new ExpandoObject();
                //    item.path = "role";
                //    item.hidden = true;
                //    result.Add(item);
                //}
            }
            return await ApiResult.Ok(result, "操作成功").Async();
        }
    }
}
