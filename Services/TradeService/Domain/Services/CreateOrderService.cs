using Domain.Dtos;
using Domain.Entities;
using Domain.ValueObject;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CreateOrderService
    {
        Func<IEnumerable<Guid>, Task<List<OrderGoodsSnapshot>>> getGoodsList;
        Func<CreateOrderDeductionGoodsStockDto, Task<bool>> deductionGoodsStock;
        Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock;
        List<CreateOrderDeductionGoodsStockDto> succGoodsIds = new List<CreateOrderDeductionGoodsStockDto>();//成功扣库存容器
        public CreateOrderService(
            Func<IEnumerable<Guid>, Task<List<OrderGoodsSnapshot>>> getGoodsList,
            Func<CreateOrderDeductionGoodsStockDto, Task<bool>> deductionGoodsStock,
            Func<CreateOrderDeductionGoodsStockDto, Task<bool>> undeductionGoodsStock)
        {
            this.getGoodsList = getGoodsList;
            this.deductionGoodsStock = deductionGoodsStock;
            this.undeductionGoodsStock = undeductionGoodsStock;
        }
        public async Task CreateOrder(Order order)
        {
            //rpc获取商品基本信息
            var goodslist = await getGoodsList(order.OrderItems.Select(x => x.GoodsId));
            //创建订单实体
            order.CreateOrder(goodslist);
            order.OrderItems.ForEach(async item =>
            {
                var deductstock = new CreateOrderDeductionGoodsStockDto(item.GoodsId, item.Count);
                var goods = goodslist.FirstOrDefault(y => y.GoodsId == item.GoodsId);
                if (!await deductionGoodsStock(deductstock))
                    throw new DomainException($"订单创建失败,商品{goods.GoodsName}库存不足");
                else
                    succGoodsIds.Add(deductstock);
            });
        }
        public async Task UnCreateOrder()
        {
            foreach (var deductstock in succGoodsIds)
            {
                await undeductionGoodsStock(deductstock);
            }
        }
    }
}
