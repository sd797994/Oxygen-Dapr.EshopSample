using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TradeLogState
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        CreateOrder = 0,
        /// <summary>
        /// 取消订单
        /// </summary>
        CancelOrder = 1,
        /// <summary>
        /// 支付订单
        /// </summary>
        PayOrder = 2,
        /// <summary>
        /// 发货
        /// </summary>
        DeliverGoods = 3,
        /// <summary>
        /// 收货
        /// </summary>
        ReceivingGoods = 4
    }
}
