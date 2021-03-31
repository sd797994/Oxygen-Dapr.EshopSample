using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AppEvent
{
    public struct EventTopicDictionary
    {
        public struct Common
        {
            public const string EventHandleErrCatch = "EventHandleErrCatch";
        }
        public struct Account
        {
            public const string InitTestUserSuccess = "InitTestUserSuccess";
            public const string LoginSucc = "AccountLoginSuccess";
            public const string Loginout = "AccountLoginOut";
            public const string LoginExpire = "AccountLoginExpire";
        }
        public struct Order
        {
            public const string CreateOrderSucc = "CreateOrderSucc";
            public const string PayOrderSucc = "PayOrderSucc";
            public const string ExpireCancelOrder = "ExpireCancelOrder";
        }
        public struct Logistics
        {
            public const string LogisticsDeliverSucc = "LogisticsDeliverSucc";
            public const string LogisticsReceiveSucc = "LogisticsReceiveSucc";
        }
        public struct Goods
        {
            public const string UpdateGoodsToEs = "UpdateGoodsToEs";
            public const string Loc_WriteToElasticsearch = "Loc_WriteToElasticsearch";
            public const string Loc_RemoveToElasticsearch = "Loc_RemoveToElasticsearch"; 
        }
    }
}
