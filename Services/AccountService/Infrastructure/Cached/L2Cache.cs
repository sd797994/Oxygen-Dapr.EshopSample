using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Store;
using Oxygen.MulitlevelCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cached
{
    public class L2Cache : IL2CacheServiceFactory
    {
        private readonly IStateManager stateManager;
        public L2Cache(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            var cache = await stateManager.GetState(new L2CacheStore(key),typeof(T));
            Console.WriteLine($"L2缓存被调用,KEY={key},value{(cache == null ? "不存在" : "存在")}");
            if (cache != null)
                return (T)cache;
            return default(T);
        }

        public async Task<bool> SetAsync<T>(string key, T value, int expireTimeSecond = 0)
        {
            var resp = await stateManager.SetState(new L2CacheStore(key, value, expireTimeSecond));
            return resp != null;
        }
    }
    internal class L2CacheStore : StateStore
    {
        public L2CacheStore(string key, object data, int expireTimeSecond = 0)
        {
            Key = $"DarpEshopL2CacheStore_{key}";
            this.Data = data;
            this.TtlInSeconds = expireTimeSecond;
        }
        public L2CacheStore(string key)
        {
            Key = $"DarpEshopL2CacheStore_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
