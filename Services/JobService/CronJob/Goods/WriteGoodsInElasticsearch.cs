using Hangfire;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.CronJob.Goods
{
    public class WriteGoodsInElasticsearch : CronJobBase
    {
        public override void RunCornJob()
        {
            RecurringJob.AddOrUpdate<IEventBus>((bus) => bus.SendEvent(EventTopicDictionary.Goods.UpdateGoodsToEs, new UpdateGoodsToEsDto()), "*/5 * * * *");
        }
    }
}
