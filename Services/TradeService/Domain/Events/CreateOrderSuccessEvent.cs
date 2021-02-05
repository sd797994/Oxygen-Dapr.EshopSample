using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class CreateOrderSuccessEvent
    {
        public CreateOrderSuccessEvent(Order order,string name)
        {
            OrderId = order.Id;
            OrderDescription = $"{name}在{DateTime.Now:yyyy年MM日dd日HH时mm分}创建了订单,订单编号:{order.OrderNo}";
            UserId = order.UserId;
        }
        public Guid OrderId { get; set; }
        public string OrderDescription { get; set; }
        public Guid UserId { get; set; }
    }
}
