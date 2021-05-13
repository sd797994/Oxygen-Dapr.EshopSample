using Domain;
using Domain.Enums;
using Domain.Events;
using Domain.Specification;
using IApplicationService;
using IApplicationService.AccountService;
using IApplicationService.AccountService.Dtos;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using ApplicationService.Dtos;
using IApplicationService.AccountService.Dtos.Output;
using Domain.Entities;
using IApplicationService.Base.AccessToken;
using IApplicationService.AccountService.Dtos.Event;

namespace ApplicationService
{
    public class AccountUseCaseService : IAccountUseCaseService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public AccountUseCaseService(IAccountRepository accountRepository, IRoleRepository roleRepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.accountRepository = accountRepository;
            this.roleRepository = roleRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        public async Task<ApiResult> InitRoleBasedAccessControler(InitUserOauthDto input)
        {
            if (!await stateManager.GetState<bool>(new RoleBaseInitCheckCache()))
            {
                await stateManager.SetState(new RoleBaseInitCheckCache(true));
                await eventBus.SendEvent(EventTopicDictionary.Account.InitTestUserSuccess, new LoginSuccessDto() { Token = input.OauthData ?? "" });
                if (!string.IsNullOrEmpty(input.OauthData))
                {
                    var data = System.Text.Json.JsonSerializer.Deserialize<InitUserOauthDto.Github>(input.OauthData);
                    await stateManager.SetState(new OauthStateStore(data));
                    return ApiResult.Ok(new DefLoginAccountResponse { LoginName = data.login, Password = "x1234567" }, $"权限初始化成功,已创建超管角色和默认登录账号");
                }
            }
            var oauth = await stateManager.GetState<InitUserOauthDto.Github>(new OauthStateStore());
            return ApiResult.Ok(new DefLoginAccountResponse { LoginName = oauth?.login ?? "eshopadmin", Password = "x1234567" }, $"权限初始化成功,已创建超管角色和默认登录账号");
        }
        public async Task<ApiResult> AccountRegister(CreateAccountDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var account = new Account();
            account.CreateAccount(input.LoginName, input.NickName, input.Password, Common.GetMD5SaltCode);
            accountRepository.Add(account);
            if (await new UniqueAccountIdSpecification(accountRepository).IsSatisfiedBy(account))
                await unitofWork.CommitAsync(tran);
            return ApiResult.Ok("用户注册成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> AccountCreate(CreateAccountDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var account = new Account();
            account.CreateAccount(input.LoginName, input.NickName, input.Password, Common.GetMD5SaltCode);
            account.SetRoles(input.Roles);
            account.User.CreateOrUpdateUser(input.User?.UserName, "", input.User?.Address, input.User?.Tel, input.User?.Gender == null ? UserGender.Unknown : (UserGender)input.User?.Gender, input.User?.BirthDay);
            accountRepository.Add(account);
            if (await new UniqueAccountIdSpecification(accountRepository).IsSatisfiedBy(account) && await new RoleValidityCheckSpecification(roleRepository).IsSatisfiedBy(account))
                await unitofWork.CommitAsync(tran);
            return ApiResult.Ok("用户创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> AccountUpdate(UpdateAccountDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var account = await accountRepository.GetAsync(input.ID);
            if (account == null)
                throw new ApplicationServiceException("所选用户不存在!");
            account.UpdateNicknameOrPassword(input.NickName, input.Password);
            account.SetRoles(input.Roles);
            account.User.CreateOrUpdateUser(input.User?.UserName, "", input.User?.Address, input.User?.Tel, input.User?.Gender == null ? UserGender.Unknown : (UserGender)input.User?.Gender, input.User?.BirthDay);
            accountRepository.Update(account);
            if (await new RoleValidityCheckSpecification(roleRepository).IsSatisfiedBy(account))
                await unitofWork.CommitAsync(tran);
            await BuildLoginCache(account);
            return ApiResult.Ok("用户信息更新成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> AccountDelete(AccountDeleteDto input)
        {
            var account = await accountRepository.GetAsync(input.AccountId);
            if (account == null)
                throw new ApplicationServiceException("所选用户不存在!");
            accountRepository.Delete(account);
            if (await new AccountDeleteCheckSpecification(HttpContextExt.Current.User.Id).IsSatisfiedBy(account))
                await unitofWork.CommitAsync();
            await stateManager.DelState(new AccountLoginCache(account.Id));
            return ApiResult.Ok("用户信息删除成功");
        }
        public async Task<ApiResult> AccountLogin(AccountLoginDto input)
        {
            var account = await accountRepository.FindAccountByAccounId(input.LoginName);
            if (account == null)
                throw new ApplicationServiceException("登录账号不存在!");
            account.CheckAccountCanLogin(Common.GetMD5SaltCode(input.Password, account.Id), input.LoginAdmin);
            await BuildLoginCache(account);
            var loginToken = Common.GetMD5SaltCode(Guid.NewGuid().ToString(), input.LoginAdmin);
            await stateManager.SetState(new AccountLoginAccessToken(loginToken, new AccessTokenItem(account.Id, input.LoginAdmin)));
            await eventBus.SendEvent(EventTopicDictionary.Account.LoginSucc, new LoginAccountSuccessEvent(loginToken));
            return ApiResult.Ok(new AccountLoginResponse(loginToken));
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> AccountLoginOut()
        {
            if (HttpContextExt.Current.User == null)
                throw new ApplicationServiceException("登录用户不存在!");
            await stateManager.DelState(new AccountLoginAccessToken(HttpContextExt.Current.User.Id.ToString()));
            return ApiResult.Ok("用户登出成功");
        }

        [AuthenticationFilter]
        public async Task<ApiResult> SupplementaryAccountInfo(SupplementaryUserDto input)
        {
            var account = await accountRepository.GetAsync(HttpContextExt.Current.User.Id);
            if (account == null)
                throw new ApplicationServiceException("登录用户不存在!");
            account.User.CreateOrUpdateUser(input.UserName, input.UserImage, input.Address, input.Tel, (UserGender)input.Gender, input.BirthDay);
            accountRepository.Update(account);
            await unitofWork.CommitAsync();
            await BuildLoginCache(account);
            return ApiResult.Ok("用户信息完善成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> LockOrUnLockAccount(LockOrUnLockAccountDto input)
        {
            var account = await accountRepository.GetAsync(input.ID);
            if (account == null)
                throw new ApplicationServiceException("所选用户不存在!");
            account.ChangeAccountLockState(HttpContextExt.Current.User.Id);
            accountRepository.Update(account);
            await unitofWork.CommitAsync();
            await BuildLoginCache(account);
            return ApiResult.Ok("用户锁定/解锁成功");
        }
        private async Task BuildLoginCache(Account account)
        {
            await stateManager.SetState(new AccountLoginCache(account.Id, new CurrentUser(account.Id, account.LoginName, account.User.UserImage, account.NickName, Convert.ToInt32(account.State), account.User.UserName, Convert.ToInt32(account.User.Gender), account.User.BirthDay, account.User.Address, account.User.Tel, await accountRepository.GetAccountPermissions(account.Id))));
        }
    }
}
