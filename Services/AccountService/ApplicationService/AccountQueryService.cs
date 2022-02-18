using ApplicationService.Dtos;
using Domain;
using Domain.Enums;
using Domain.Repository;
using IApplicationService;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using InfrastructureBase.Data.Nest;
using System.Net.Http;
using Infrastructure.Filter;
using Oxygen.MulitlevelCache;
using System.Threading;

namespace ApplicationService
{
    public class AccountQueryService : IAccountQueryService
    {
        private readonly IStateManager stateManager;
        private readonly EfDbContext efDbContext;
        private readonly IHttpClientFactory httpClientFactory;
        public AccountQueryService(IStateManager stateManager, EfDbContext efDbContext, IHttpClientFactory httpClientFactory)
        {
            this.stateManager = stateManager;
            this.efDbContext = efDbContext;
            this.httpClientFactory = httpClientFactory;
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetAccountInfo()
        {
            return await ApiResult.Ok(HttpContextExt.Current.User).Async();
        }
        [TestMethodFilter]
        public async Task<ApiResult> CheckRoleBasedAccessControler()
        {
            if (await stateManager.GetState<bool>(new RoleBaseInitCheckCache()))
            {
                var oauth = await stateManager.GetState<InitUserOauthDto.Github>(new OauthStateStore());
                return ApiResult.Ok(new DefLoginAccountResponse { LoginName = oauth?.login ?? "eshopadmin", Password = "x1234567" });
            }
            else
                return ApiResult.Ok(false);
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAccountList(PageQueryInputBase input)
        {
            var query = (from account in efDbContext.Account
                         join user in efDbContext.User on account.Id equals user.AccountId
                         select new GetAccountListResponse
                         {
                             Id = account.Id,
                             LoginName = account.LoginName,
                             NickName = account.NickName,
                             State = (int)account.State,
                             Gender = (int)user.Gender,
                             BirthDay = user.BirthDay,
                             Address = user.Address,
                             Tel = user.Tel,
                             UserName = user.UserName
                         }).OrderBy(x => x.LoginName);
            var (Data, Total) = await QueryServiceHelper.PageQuery(query, input.Page, input.Limit);
            var accoundIds = Data.Select(x => x.Id).ToList();
            var roles = await (from role in efDbContext.Role
                               join userrole in efDbContext.UserRole.Where(ur => accoundIds.Contains(ur.AccountId)) on role.Id equals userrole.RoleId
                               select new { userrole.AccountId, role.Id, role.RoleName, role.SuperAdmin }).AsNoTracking().ToListAsync();
            Data.ForEach(account =>
            {
                account.Roles = roles.Where(role => role.AccountId == account.Id).Select(role => new GetAccountListResponse.RoleItem() { RoleId = role.Id, RoleName = role.RoleName, SuperAdmin = role.SuperAdmin });
            });
            return ApiResult.Ok(new PageQueryResonseBase<GetAccountListResponse>(Data, Total));
        }

        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetAccountUserNameByIds(GetAccountUserNameByIdsDto input)
        {
            var query = efDbContext.User.Where(x => input.Ids.Contains(x.AccountId)).Select(x => new GetAccountUserNameByIdsResponse { AccountId = x.AccountId, Name = x.UserName }).ToListAsync();
            return await ApiResult.Ok(query).Async();
        }
        [SystemCached]
        public async Task<ApiResult> GetMockAccount()
        {
            var result = await (from account in efDbContext.Account.Where(x => x.LoginName == "eshopuser")
                                join user in efDbContext.User on account.Id equals user.AccountId
                                select new CurrentUser
                                {
                                    Id = account.Id,
                                    LoginName = account.LoginName,
                                    UserImage = user.UserImage,
                                    NickName = account.NickName,
                                    State = (int)account.State,
                                    UserName = user.UserName,
                                    Gender = (int)user.Gender,
                                    BirthDay = user.BirthDay,
                                    Address = user.Address,
                                    Tel = user.Tel
                                }).FirstOrDefaultAsync();
            return ApiResult.Ok(result);
        }

    }
}
