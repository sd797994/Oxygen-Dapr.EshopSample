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
        //商品快照
        //商品价格
        public decimal TotalPrice { get; set; }
        //商品数量
        public int Count { get; set; }
    }
}
