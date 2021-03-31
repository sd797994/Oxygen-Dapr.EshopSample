using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class GetOrderSellCountByGoodsIdDto
    {
        public Guid[] GoodsIds { get; set; }
        public DateTime ExpressTime { get; set; }
    }
}
