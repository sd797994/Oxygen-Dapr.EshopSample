using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.Hosting;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
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
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    Thread.Sleep(20000);//等待sidercar启动
                    var sender = await eventBus.SendEvent(EventTopicDictionary.Common.InitAuthApiList, new InitPermissionApiEvent<List<AuthenticationInfo>>(AuthenticationManager.AuthenticationMethods));//将当前服务的需鉴权接口发送给用户服务
                    if (sender != default(DefaultResponse))
                        break;
                    else
                        Console.WriteLine("事件初始化失败，20秒后重试!");
                }
            });
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
