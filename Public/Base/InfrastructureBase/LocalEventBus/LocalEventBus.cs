using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    public class LocalEventBus: ILocalEventBus
    {
        public async Task SendEvent<T>(string topic, params T[] datas)
        {
            var channel = ChannelDictionary.GetChannel<T>(topic);
            if (channel != null)
            {
                foreach (var item in datas)
                {
                    await channel.Writer.WriteAsync(item);
                }
            }
            else
            {
                throw new Exception("未注册事件订阅器!");
            }
        }

        public async Task SendEvent<T>(string topic, List<T> datas)
        {
            var channel = ChannelDictionary.GetChannel<T>(topic);
            if (channel != null)
            {
                foreach (var item in datas)
                {
                    await channel.Writer.WriteAsync(item);
                }
            }
            else
            {
                throw new Exception("未注册事件订阅器!");
            }
        }
    }
}
