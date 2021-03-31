using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class OrderUpdateDto: OrderCreateDto
    {
        public Guid Id { get; set; }
    }
}
