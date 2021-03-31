using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    public interface ILocalEventBus
    {
        Task SendEvent<T>(string topic, params T[] datas);
        Task SendEvent<T>(string topic, List<T> datas);
    }
}
