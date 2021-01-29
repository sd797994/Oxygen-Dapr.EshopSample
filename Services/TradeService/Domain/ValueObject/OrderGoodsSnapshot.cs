using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject
{
    /// <summary>
    /// 订单详情-商品快照
    /// </summary>
    public class OrderGoodsSnapshot
    {
        public Guid GoodsId { get; set; }
        public string CategoryName { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImage { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
    }
}
