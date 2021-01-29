using Domain.Enums;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        //订单号
        public string OrderNo { get; set; }
        //订单总价
        public decimal TotalPrice { get; set; }
        //订单详情
        public List<OrderItem> OrderItems { get; set; }
        //订单状态
        public OrderState OrderState { get; set; }
        //下单时间
        public DateTime CreateTime { get; set; }

        //创建订单
        public void CreateOrder(List<OrderItem> orderItems)
        {
            if (!orderItems.Any())
                throw new DomainException("订单详情不能为空!");
            OrderNo = CreateOrderNo();
            TotalPrice = orderItems.Sum(x => x.TotalPrice);
            OrderState = OrderState.Create;
            CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        string CreateOrderNo()
        {
            return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999)}";
        }
        //变更订单状态
        public void ChangeOrderState(OrderState orderState)
        {

        }
    }
}
