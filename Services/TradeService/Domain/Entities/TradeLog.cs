using Domain.Enums;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TradeLog : Entity, IAggregateRoot
    {
        //订单编号
        public Guid OrderId { get; set; }
        public TradeLogState State { get; set; }
        public string OrderNo { get; set; }
        //物流编号
        public Guid? LogisticsId { get; set; }
        public string LogisticsNo { get; set; }
        public Guid OperateUserId { get; set; }
        public string OperateUserName { get; set; }
        //记录时间
        public DateTime TradeDate { get; set; }
        //记录事件
        public string TradeLogValue { get; set; }
        /// <summary>
        /// 创建交易流水
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderNo"></param>
        /// <param name="logisticsId"></param>
        /// <param name="tradeLogValue"></param>
        public void CreateTradeLog(TradeLogState State, Guid orderId, string orderNo, Guid? logisticsId, string logisticsNo, Guid operateUserId, string operateUserName)
        {

            TradeDate = DateTime.Now;
            this.State = State;
            if (orderId == Guid.Empty)
                throw new DomainException("订单号不存在!");
            OrderId = orderId;
            if (string.IsNullOrEmpty(orderNo))
                throw new DomainException("订单编号不能为空!");
            OrderNo = orderNo;
            if (logisticsId != null && logisticsId != Guid.Empty)
            {
                LogisticsId = logisticsId;
                if (string.IsNullOrEmpty(logisticsNo))
                    throw new DomainException("物流编号不能为空!");
                LogisticsNo = logisticsNo;
            }
            OperateUserId = operateUserId;
            if (OperateUserId == Guid.Empty)
                OperateUserName = "[系统]";
            else if (string.IsNullOrEmpty(operateUserName))
                throw new DomainException("操作人员不能为空!");
            else
                OperateUserName = $"用户[{operateUserName}]";
            TradeLogValue = CreateTradeLogValueByState();
        }
        /// <summary>
        /// 创建事件描述
        /// </summary>
        /// <returns></returns>
        private string CreateTradeLogValueByState()
        {
            switch (State)
            {
                case TradeLogState.CreateOrder:
                    return $"{OperateUserName}在{TradeDate:yyyy年MM日dd日HH时mm分}创建了订单,订单编号:{OrderNo}";
                case TradeLogState.CancelOrder:
                    return $"由于超时未支付,{OperateUserName}在{TradeDate:yyyy年MM日dd日HH时mm分}取消了订单,订单编号:{OrderNo}";
                case TradeLogState.PayOrder:
                    return $"{OperateUserName}在{TradeDate:yyyy年MM日dd日HH时mm分}支付了订单,订单编号:{OrderNo}";
                case TradeLogState.DeliverGoods:
                    return $"{OperateUserName}在{TradeDate:yyyy年MM日dd日HH时mm分}进行了发货,订单编号:{OrderNo},物流单号:{LogisticsNo}";
                case TradeLogState.ReceivingGoods:
                    return $"{OperateUserName}在{TradeDate:yyyy年MM日dd日HH时mm分}确认了收货,订单编号:{OrderNo},物流单号:{LogisticsNo}";
            }
            throw new DomainException("错误的交易流水状态!");
        }
    }
}
