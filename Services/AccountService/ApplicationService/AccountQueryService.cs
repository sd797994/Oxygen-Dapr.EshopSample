using ApplicationService.Dtos;
using Domain;
using Domain.Enums;
using Domain.Repository;
using IApplicationService;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AccountService.Dtos.Output;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Data;
using InfrastructureBase.Http;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class AccountQueryService : IApplicationService.AccountService.AccountQueryService
    {
        private readonly IStateManager stateManager;
        private readonly EfDbContext efDbContext;
        public AccountQueryService(IStateManager stateManager, EfDbContext efDbContext)
        {
            this.stateManager = stateManager;
            this.efDbContext = efDbContext;
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> GetAccountInfo()
        {
            var token = HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key == "Authentication").Value;
            var userid = await stateManager.GetState<Guid>(new AccountLoginAccessToken(token));
            if (userid == Guid.Empty)
                throw new ApplicationServiceException("授权登录Token已过期,请重新登录!");
            var userinfo = await stateManager.GetState<CurrentUser>(new AccountLoginCache(userid));
            if (userinfo == null)
                throw new ApplicationServiceException("登录用户信息已过期,请重新登录!");
            else if (userinfo.State == Convert.ToInt32(AccountState.Locking))
                throw new ApplicationServiceException("登录用户已被锁定,请重新登录!");
            return ApiResult.Ok(userinfo);
        }
        public async Task<ApiResult> CheckRoleBasedAccessControler()
        {
            if (await stateManager.GetState<bool>(new RoleBaseInitCheckCache()))
                return ApiResult.Ok(new DefLoginAccountResponse { LoginName = "superadmin", Password = "x1234567" });
            else
                return ApiResult.Ok(false);
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetAccountList(PageQueryInputBase input)
        {
            var query = from account in efDbContext.Account
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
                        };
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
    }
}