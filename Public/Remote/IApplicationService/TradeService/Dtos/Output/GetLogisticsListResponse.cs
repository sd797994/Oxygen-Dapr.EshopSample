using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Output
{
    public class GetLogisticsListResponse
    {
        public Guid OrderId { get; set; }
        public Guid? Id { get; set; }
        public string OrderNo { get; set; }
        public int? LogisticsType { get; set; }
        public string LogisticsNo { get; set; }
        public string DeliverName { get; set; }
        public string DeliverAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public DateTime? DeliveTime { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public int? LogisticsState { get; set; }
    }
}
