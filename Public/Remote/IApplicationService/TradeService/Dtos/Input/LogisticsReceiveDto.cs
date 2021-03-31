using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class LogisticsReceiveDto
    {
        public Guid LogisticsId { get; set; }
        public DateTime? ReceiveTime { get; set; }
    }
}
