using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CancelOrderService
    {
        Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock;
        public CancelOrderService(
            Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock)
        {
            this.undeductionGoodsStock = undeductionGoodsStock;
        }
        public async Task<bool> Cancel(Order order)
        {
            if(order.OrderState == Enums.OrderState.Create)
            {
                foreach (var item in order.OrderItems)
                {
                    var deductstock = new CreateOrderDeductionGoodsStockDto(item.GoodsId, item.Count);
                    await undeductionGoodsStock(deductstock);
                }
                order.OrderState = Enums.OrderState.Cancel;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
