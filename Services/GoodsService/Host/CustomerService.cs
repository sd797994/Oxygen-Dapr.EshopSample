using Autofac;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using Infrastructure.Elasticsearch;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Host
{
    public class CustomerService : IHostedService
    {
        private readonly EfDbContext dbContext;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public CustomerService(EfDbContext dbContext, IEventBus eventBus, IStateManager stateManager,ILifetimeScope lifetimeScope)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            //注册本地事件总线订阅器
            EventHandleRunner.RegisterAndRun<IEsGoodsRepository, Domain.Entities.Goods>(lifetimeScope, EventTopicDictionary.Goods.Loc_WriteToElasticsearch, nameof(IEsGoodsRepository.WriteToElasticsearch));
            EventHandleRunner.RegisterAndRun<IEsGoodsRepository, Domain.Entities.Goods>(lifetimeScope, EventTopicDictionary.Goods.Loc_RemoveToElasticsearch, nameof(IEsGoodsRepository.RemoveToElasticsearch));
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            dbContext.Database.EnsureCreated();//自动迁移数据库
            //阻塞20秒等待daprd启动后推送权限数据到state
            _ = Task.Delay(20 * 1000).ContinueWith(async t => await stateManager.SetState(new PermissionState() { Key = "goods", Data = AuthenticationManager.AuthenticationMethods }));
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
