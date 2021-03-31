using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    internal class ChannelDictionary
    {
        internal static Dictionary<string, dynamic> ChannelDir = new Dictionary<string, dynamic>();
        internal static Channel<T> GetChannel<T>(string topic)
        {
            ChannelDir.TryGetValue(topic, out dynamic value);
            return value;
        }

        internal static void SetChannel<T>(string topic, Channel<T> channel)
        {
            ChannelDir.TryAdd(topic, channel);
        }
    }
}
