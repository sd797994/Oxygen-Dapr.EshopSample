using Autofac;
using IApplicationService.Base.AccessToken;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AuthenticationHandler : AuthenticationManager
    {
        public static new void RegisterAllFilter()
        {
            AuthenticationManager.RegisterAllFilter();
        }
        public override async Task AuthenticationCheck(string routePath)
        {
            var authMethod = AuthenticationMethods.FirstOrDefault(x => x.Path.Equals(routePath));
            if (authMethod != null)
            {
                var token = HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key == "Authentication").Value;
                var accountInfo = await GetAccountInfo(HttpContextExt.Current.RequestService.Resolve<IStateManager>());
                HttpContextExt.SetUser(accountInfo);
                if (!HttpContextExt.Current.User.IgnorePermission && authMethod.CheckPermission && !HttpContextExt.Current.GetAuthIgnore() && HttpContextExt.Current.User.Permissions != null && !HttpContextExt.Current.User.Permissions.Contains(routePath))
                    throw new InfrastructureException("当前登录用户缺少使用该接口的必要权限,请重试!");
            }
        }

        private async Task<CurrentUser> GetAccountInfo(IStateManager stateManager)
        {
            var token = HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key == "Authentication").Value;
            var usertoken = await stateManager.GetState<AccessTokenItem>(new AccountLoginAccessToken(token));
            if (usertoken == null)
                throw new InfrastructureException("授权登录Token已过期,请重新登录!");
            var userinfo = await stateManager.GetState<CurrentUser>(new AccountLoginCache(usertoken.Id));
            if (userinfo == null)
                throw new InfrastructureException("登录用户信息已过期,请重新登录!");
            else if (userinfo.State == 1)
                throw new InfrastructureException("登录用户已被锁定,请重新登录!");
            if (!usertoken.LoginAdmin)
                userinfo.Permissions = null;
            return userinfo;
        }
    }
}