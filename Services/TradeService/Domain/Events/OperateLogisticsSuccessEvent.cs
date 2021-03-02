using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class OperateLogisticsSuccessEvent
    {
        public OperateLogisticsSuccessEvent(Logistics logistics, string name)
        {
            LogisticsId = logistics.Id;
            OrderId = logistics.OrderId;
            UserId = logistics.LogisticsState == LogisticsState.DeliverGoods ? logistics.DeliverUserId : logistics.ReceiverUserId;
            UserName = name;
        }
        public Guid LogisticsId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
