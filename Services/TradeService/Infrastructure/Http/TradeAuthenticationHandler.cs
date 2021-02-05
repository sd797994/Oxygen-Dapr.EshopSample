using Autofac;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Http
{
    public class TradeAuthenticationHandler : AuthenticationManager
    {
        public static new void RegisterAllFilter()
        {
            AuthenticationManager.RegisterAllFilter();
        }
        public override async Task AuthenticationCheck(string routePath)
        {
            if (AuthenticationMethods.Any(x => x.Path.Equals(routePath)))
            {
                var accountInfo = await HttpContextExt.Current.RequestService.Resolve<IServiceProxyFactory>().CreateProxy<IApplicationService.AccountService.IAccountQueryService>().GetAccountInfo();
                if (accountInfo.Code != 0)
                    throw new InfrastructureException(accountInfo.Message);
                HttpContextExt.SetUser(accountInfo.GetData<CurrentUser>());
                if (!HttpContextExt.Current.User.IgnorePermission && !HttpContextExt.Current.GetAuthIgnore() && !HttpContextExt.Current.User.Permissions.Contains(routePath))
                    throw new InfrastructureException("当前登录用户缺少使用该接口的必要权限,请重试!");
            }
        }
    }
}
