using Autofac;
using IApplicationService.AccountService;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class AccountAuthenticationHandler : AuthenticationManager
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
                var accountInfo = await HttpContextExt.Current.RequestService.Resolve<AccountQueryService>().GetAccountInfo();
                HttpContextExt.SetUser(accountInfo.GetData<CurrentUser>());
                if (!HttpContextExt.Current.User.IgnorePermission && authMethod.CheckPermission && !HttpContextExt.Current.GetAuthIgnore() && !HttpContextExt.Current.User.Permissions.Contains(routePath))
                {
                    throw new InfrastructureException("当前登录用户缺少使用该接口的必要权限,请重试!");
                }
            }
        }
    }
}