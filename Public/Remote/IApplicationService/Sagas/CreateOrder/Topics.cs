using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Sagas.CreateOrder
{
    public struct Topics
    {
        public struct GoodsHandler
        {
            const string ServiceName = "GoodsService";
            /// <summary>
            /// 预扣库存
            /// </summary>
            public const string PreDeductInventory = $"{ServiceName}.PreDeductInventory";
            /// <summary>
            /// 库存回滚
            /// </summary>
            public const string InventoryRollback = $"{ServiceName}.InventoryRollback";

        }
        public struct OrderHandler
        {
            const string ServiceName = "OrderService";
            /// <summary>
            /// 订单生成
            /// </summary>
            public const string OrderCreate = $"{ServiceName}.OrderCreate";
        }
    }
}
