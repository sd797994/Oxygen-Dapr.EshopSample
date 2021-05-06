using ApplicationService.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Domain.Specification;
using IApplicationService.AccountService.Dtos;
using IApplicationService.AccountService.Dtos.Event;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using IApplicationService.Base.AccessToken;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Server.Kestrel.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class AccountEventHandler : IEventHandler
    {
        private readonly IStateManager stateManager;
        private readonly IAccountRepository accountRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitofWork unitofWork;
        public AccountEventHandler(IStateManager stateManager, IAccountRepository accountRepository, IRoleRepository roleRepository, IUnitofWork unitofWork)
        {
            this.stateManager = stateManager;
            this.accountRepository = accountRepository;
            this.roleRepository = roleRepository;
            this.unitofWork = unitofWork;
        }
        [EventHandlerFunc(EventTopicDictionary.Account.LoginExpire)]
        public async Task<DefaultEventHandlerResponse> LoginCacheExpire(EventHandleRequest<LoginSuccessDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(LoginCacheExpire), input.GetDataJson(), async () =>
            {
                await stateManager.DelState(new AccountLoginAccessToken(input.GetData().Token));
            });
        }

        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> InitRoleBasedAccessControler(EventHandleRequest<LoginSuccessDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(InitRoleBasedAccessControler), input.GetDataJson(), async () =>
            {
                InitUserOauthDto.Github data = default;
                if (!string.IsNullOrEmpty(input.GetData().Token))
                {
                    data = System.Text.Json.JsonSerializer.Deserialize<InitUserOauthDto.Github>(input.GetData().Token);
                }
                var role = new Role();
                role.SetRole("超级管理员", true);
                roleRepository.Add(role);
                var admin = new Account();
                var defpwd = "x1234567";
                admin.CreateAccount(data?.login ?? "eshopadmin", data?.name ?? "商城管理员", defpwd, Common.GetMD5SaltCode);
                if (data != null)
                    admin.User.CreateOrUpdateUser(data?.name ?? "商城管理员", data?.avatar_url ?? "", "", "", UserGender.Male, Convert.ToDateTime("1980-01-01"));
                admin.SetRoles(new List<Guid>() { role.Id });
                var defbuyer = new Account();
                defbuyer.CreateAccount("eshopuser", "白云苍狗", defpwd, Common.GetMD5SaltCode);
                defbuyer.User.CreateOrUpdateUser("张老三", "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fhbimg.huabanimg.com%2F0830450561b24f4573bed70d7f74bd43f39302e11bee-s2tj6i_fw658&refer=http%3A%2F%2Fhbimg.huabanimg.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1618110799&t=b215598f3b458ad7c08aee2b4614622b", "北京市海淀区太平路1号", "13000000000", UserGender.Male, Convert.ToDateTime("1980-01-01"));
                accountRepository.Add(admin);
                accountRepository.Add(defbuyer);
                if (await new UniqueSuperRoleSpecification(roleRepository).IsSatisfiedBy(role))
                    await unitofWork.CommitAsync();
                await stateManager.SetState(new RoleBaseInitCheckCache(true));
            });
        }
    }
}
