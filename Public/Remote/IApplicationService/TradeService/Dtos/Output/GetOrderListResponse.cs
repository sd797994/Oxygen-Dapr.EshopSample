using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Output
{
    public class GetOrderListResponse
    {
        public Guid OrderId { get; set; }
        //订单号
        public string OrderNo { get; set; }
        //订单状态
        public int OrderState { get; set; }
        //订单总价
        public decimal TotalPrice { get; set; }
        //订单明细
        public IEnumerable<GetOrderListItemResponse> OrderItems { get; set; }
        //下单人
        public string UserName { get; set; }
        //下单时间
        public DateTime CreateTime { get; set; }
    }
    public class GetOrderListItemResponse
    {
        //商品名
        public string GoodsName { get; set; }
        //商品价格
        public decimal Price { get; set; }
        //商品数量
        public int Count { get; set; }
    }
}
