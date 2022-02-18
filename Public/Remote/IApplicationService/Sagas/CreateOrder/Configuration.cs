using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Sagas.CreateOrder
{
    public class Configuration : TopicConfiguration
    {
        public override string FlowName { get; set; } = "CreateOrder";
        public Configuration()
        {
            AddNext(Topics.GoodsHandler.PreDeductInventory, Topics.GoodsHandler.InventoryRollback)
                .AddNext(Topics.OrderHandler.OrderCreate);
        }
    }
}
