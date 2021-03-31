using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Client.ServerSymbol.Store;
using Oxygen.Common.Interface;
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
        public CustomerService(EfDbContext dbContext, IEventBus eventBus, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            dbContext.Database.EnsureCreated();//自动迁移数据库
            await stateManager.SetState(new PermissionState() { Key = "account", Data = AuthenticationManager.AuthenticationMethods });
            await Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
