using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Event
{
    public class CreateLogisticsSuccDto
    {
        public Guid LogisticsId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
