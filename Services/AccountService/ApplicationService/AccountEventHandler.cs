using ApplicationService.Dtos;
using IApplicationService.AccountService.Dtos;
using IApplicationService.AccountService.Dtos.Event;
using IApplicationService.AccountService.Dtos.Input;
using IApplicationService.AppEvent;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
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
        public AccountEventHandler(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        [EventHandlerFunc(EventTopicDictionary.Account.LoginExpire)]
        public async Task<DefaultEventHandlerResponse> LoginCacheExpire(EventHandleRequest<LoginSuccessDto> input)
        {
            await stateManager.DelState(new AccountLoginAccessToken(input.data.Token));
            return await Task.FromResult(DefaultEventHandlerResponse.Default());
        }
    }
}
