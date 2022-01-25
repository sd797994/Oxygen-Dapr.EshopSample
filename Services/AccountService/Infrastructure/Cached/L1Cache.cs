using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Oxygen.MulitlevelCache;
namespace Infrastructure.Cached
{
    public class L1Cache : IL1CacheServiceFactory
    {
        private readonly IMemoryCache memoryCache;
        public L1Cache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public T Get<T>(string key)
        {
            Console.WriteLine($"L1缓存被调用,KEY={key},value{(memoryCache.Get<T>(key) == null ? "不存在" : "存在")}");
            return memoryCache.Get<T>(key);
        }

        public bool Set<T>(string key, T value, int expireTimeSecond = 0)
        {
            return memoryCache.Set(key, value, DateTimeOffset.Now.AddSeconds(expireTimeSecond)) != null;
        }
    }
}
