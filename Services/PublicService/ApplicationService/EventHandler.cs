using Domain.Entities;
using Domain.Repository;
using IApplicationService.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using Infrastructure.EfDataAccess;
using InfrastructureBase.Object;
using Microsoft.Extensions.Logging;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class EventHandler : IEventHandler
    {
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly ILogger<EventHandler> logger;
        private readonly IEventHandleErrorInfoRepository infoRepository;
        public EventHandler(IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork, ILogger<EventHandler> logger, IEventHandleErrorInfoRepository infoRepository)
        {
            this.logger = logger;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.infoRepository = infoRepository;
        }
        [EventHandlerFunc(EventTopicDictionary.Common.EventHandleErrCatch)]
        public async Task<DefaultEventHandlerResponse> EventHandleErrCatch(EventHandleRequest<EventHandlerErrDto> input)
        {
            try
            {
                var entity = input.GetData().CopyTo<EventHandlerErrDto, EventHandleErrorInfo>();
                infoRepository.Add(entity);
                await unitofWork.CommitAsync();
            }
            catch (Exception e)
            {
                logger.LogError($"事件订阅器异常处理持久化失败,异常信息:{e.Message}");
            }
            return DefaultEventHandlerResponse.Default();
        }
    }
}
