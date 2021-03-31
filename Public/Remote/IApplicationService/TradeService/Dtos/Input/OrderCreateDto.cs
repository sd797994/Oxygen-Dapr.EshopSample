using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class OrderCreateDto
    {
        public List<OrderCreateItemDto> Items { get; set; }
        public class OrderCreateItemDto
        {
            public Guid GoodsId { get; set; }
            public int Count { get; set; }
        }
    }
}
