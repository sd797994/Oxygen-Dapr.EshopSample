using IApplicationService.Sagas.CreateOrder.Dtos;
using IApplicationService.TradeService.Dtos.Input;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Sagas.CreateOrder.Handles
{
    public interface IOrderHandle
    {

        [SagaLogicHandler(Topics.OrderHandler.OrderCreate, HandleType.Handle)]
        Task OrderCreate(OrderCreateDto dto);
    }
}
