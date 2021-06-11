using IApplicationService.Base.SentinelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.AliSentinel.Extension
{
    public static class AliSentinelConfigExtension
    {
        /// <summary>
        /// 去重取最后一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliSentinelConfigs"></param>
        /// <param name="newSentinelConfig"></param>
        public static List<T> GetDistinct<T>(this List<T> aliSentinelConfigs) where T : AliSentinelConfig,new()
        {
            if (aliSentinelConfigs == null)
                return new List<T>();
            return aliSentinelConfigs.GroupBy(x => x.Resource).Select(x => x.LastOrDefault()).ToList();
        }
    }
}
