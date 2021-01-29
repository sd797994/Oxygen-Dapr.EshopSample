using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CreateOrderDeductionGoodsStockDto
    {
        public CreateOrderDeductionGoodsStockDto(Guid goodsId, int count)
        {
            GoodsId = goodsId;
            Count = count;
        }
        public Guid GoodsId { get; set; }
        public int Count { get; set; }
    }
}
