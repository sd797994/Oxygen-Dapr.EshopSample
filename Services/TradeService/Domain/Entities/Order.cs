using Domain.Dtos;
using Domain.Enums;
using Domain.ValueObject;
using DomainBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public IEnumerable<OrderItem> OrderItems { get; set; }
        //订单状态
        public OrderState OrderState { get; set; }
        //下单人
        public Guid UserId { get; set; }
        //下单时间
        public DateTime CreateTime { get; set; }
        
        public OrderConsigneeInfo ConsigneeInfo { get; set; }
        //创建订单
        public void CreateOrder(Guid userId, string consigneeName, string consigneeAddress, string consigneeTel, IEnumerable<OrderItem> orderItems)
        {
            if (string.IsNullOrEmpty(consigneeName) || string.IsNullOrEmpty(consigneeAddress) || string.IsNullOrEmpty(consigneeTel))
                throw new DomainException("收件人信息缺失,请补全收件人信息再进行下单操作!");
            ConsigneeInfo = new OrderConsigneeInfo()
            {
                Name = consigneeName,
                Address = consigneeAddress,
                Tel = consigneeTel
            };

            if (!orderItems.Any())
                throw new DomainException("订单明细不能为空!");
            OrderNo = CreateOrderNo();
            TotalPrice = orderItems.Sum(x => x.TotalPrice);
            OrderState = OrderState.Create;
            UserId = userId;
            OrderItems = orderItems;
            CreateTime = DateTime.Now;
        }
        public void PayOrder(Guid userId)
        {
            if (OrderState != OrderState.Create)
                throw new DomainException("当前订单状态无法支付,请刷新后再试");
            if (UserId != userId)
                throw new DomainException("你无法对该订单进行支付");
            OrderState = OrderState.Pay;
        }
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        string CreateOrderNo()
        {
            return $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999)}";
        }
        /// <summary>
        /// 取消当前订单
        /// </summary>
        public bool CancelOrder()
        {
            if (OrderState == OrderState.Create)
            {
                OrderState = OrderState.Cancel;
                return true;
            }
            return false;
        }
    }
}
