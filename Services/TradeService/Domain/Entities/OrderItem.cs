using Domain.ValueObject;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem : Entity
    {
        //订单编号
        public Guid OrderId { get; set; }
        public Guid GoodsId { get; set; }
        //商品快照
        public OrderGoodsSnapshot GoodsSnapshot { get; set; }
        //商品价格
        public decimal TotalPrice { get; set; }
        //商品数量
        public int Count { get; set; }
        public void Create(Guid orderId, OrderGoodsSnapshot goodsSnapshot)
        {
            OrderId = orderId;
            GoodsSnapshot = goodsSnapshot;
            TotalPrice = Count * GoodsSnapshot.Price;
        }
    }
}