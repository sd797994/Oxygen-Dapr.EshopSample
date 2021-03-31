using System;
using System.Linq;
using System.Threading.Tasks;
using IApplicationService;
using IApplicationService.TradeService;
using IApplicationService.Base.AppQuery;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.PersistenceObject;
using IApplicationService.TradeService.Dtos.Output;
using IApplicationService.TradeService.Dtos.Input;
using Domain.Repository;
using InfrastructureBase;
using Domain.Entities;
using InfrastructureBase.Object;
using Logistics = Domain.Entities.Logistics;
using Domain.Enums;
using IApplicationService.PublicService;
using IApplicationService.PublicService.Dtos.Output;
using Domain.Specification;
using IApplicationService.AppEvent;
using Domain.Events;
using InfrastructureBase.Http;

namespace ApplicationService
{
    public class LogisticsUseCaseService : ILogisticsUseCaseService
    {
        private readonly ILogisticsRepository repository;
        private readonly IOrderRepository orderRepository;
        private readonly IMallSettingQueryService mallSettingQuery;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public LogisticsUseCaseService(ILogisticsRepository repository, IOrderRepository orderRepository, IMallSettingQueryService mallSettingQuery, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.orderRepository = orderRepository;
            this.mallSettingQuery = mallSettingQuery;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter(false)]
        public async Task<ApiResult> Deliver(LogisticsDeliverDto input)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            var order = await orderRepository.GetAsync(input.OrderId);
            if (order == null)
                throw new ApplicationServiceException("订单无效!");
            var logistics = new Logistics();
            var mallSetting = (await mallSettingQuery.GetMallSetting()).GetData<MallSettingOutInfo>();
            logistics.Deliver(input.OrderId, (LogisticsType)input.LogisticsType, input.LogisticsNo, mallSetting.DeliverName, mallSetting.DeliverAddress, HttpContextExt.Current.User.Id, order.ConsigneeInfo.Name, order.ConsigneeInfo.Address, input.DeliveTime);
            repository.Add(logistics);
            if (await new LogisticsDeliverCheckSpecification(repository).IsSatisfiedBy(logistics))
            {
                await unitofWork.CommitAsync(tran);
                await eventBus.SendEvent(EventTopicDictionary.Logistics.LogisticsDeliverSucc, new OperateLogisticsSuccessEvent(logistics, HttpContextExt.Current.User.LoginName));
            }
            return ApiResult.Ok();
        }

        [AuthenticationFilter(false)]
        public async Task<ApiResult> Receive(LogisticsReceiveDto input)
        {
            var logistics = await repository.GetAsync(input.LogisticsId);
            if (logistics == null)
                throw new ApplicationServiceException("物流单无效!");
            logistics.Receive(HttpContextExt.Current.User.Id, input.ReceiveTime);
            repository.Update(logistics);
            await unitofWork.CommitAsync();
            await eventBus.SendEvent(EventTopicDictionary.Logistics.LogisticsReceiveSucc, new OperateLogisticsSuccessEvent(logistics, HttpContextExt.Current.User.LoginName));
            return ApiResult.Ok();
        }
    }
}
