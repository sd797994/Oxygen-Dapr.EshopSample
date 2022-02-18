using IApplicationService.Sagas.CreateOrder.Dtos;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Sagas.CreateOrder.Handles
{
    public interface IGoodsHandler
    {
        [SagaLogicHandler(Topics.GoodsHandler.PreDeductInventory, HandleType.Handle)]
        Task<DeductionStockDto> DeductInventory(DeductionStockDto dto);
        [SagaLogicHandler(Topics.GoodsHandler.InventoryRollback, HandleType.Rollback)]
        Task InventoryRollback(DeductionStockDto dto);
    }
}
