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
        private readonly IMallSettingRepository mallsettingRepository;
        public EventHandler(IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork, ILogger<EventHandler> logger, IEventHandleErrorInfoRepository infoRepository, IMallSettingRepository mallsettingRepository)
        {
            this.logger = logger;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.infoRepository = infoRepository;
            this.mallsettingRepository = mallsettingRepository;
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
        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> EventHandleSetDefMallSetting(EventHandleRequest<string> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleSetDefMallSetting), input.GetDataJson(), async () =>
            {
                var entity = new MallSetting();
                entity.CreateOrUpdate("粥品香坊", "蜂鸟专送/38分钟送达", "http://static.galileo.xiaojukeji.com/static/tms/seller_avatar_256px.jpg", "粥品香坊其烹饪粥料的秘方源于中国千年古法，在融和现代制作工艺，由世界烹饪大师屈浩先生领衔研发。坚守纯天然、0添加的良心品质深得消费者青睐，发展至今成为粥类的引领品牌。是2008年奥运会和2013年园博会指定餐饮服务商。", "李老二", "北京市海淀区太平路13号粥品香坊");
                mallsettingRepository.Add(entity);
                await unitofWork.CommitAsync();
            });
        }
    }
}
