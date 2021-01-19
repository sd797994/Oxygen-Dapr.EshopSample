using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.Hosting;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Host
{
    public class CustomerService : IHostedService
    {
        private readonly EfDbContext dbContext;
        private readonly IEventBus eventBus;
        public CustomerService(EfDbContext dbContext, IEventBus eventBus)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            dbContext.Database.EnsureCreated();//自动迁移数据库
            await eventBus.SendEvent(EventTopicDictionary.Common.InitAuthApiList, new InitPermessionApiEvent(AuthenticationManager.AuthenticationMethods));//将当前服务的需鉴权接口发送给用户服务
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
