using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Event
{
    public class OperateOrderSuccDto
    {
        public Guid OrderId { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
    }
}
