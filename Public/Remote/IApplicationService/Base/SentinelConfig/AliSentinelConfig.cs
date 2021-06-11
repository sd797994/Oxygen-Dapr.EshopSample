using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.SentinelConfig
{
    /// <summary>
    /// sentinel组件配置基类
    /// </summary>
    public abstract class AliSentinelConfig
    {
        public string ServiceName { get; set; }
        public string PathName { get; set; }
        public string Resource
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ServiceName) || string.IsNullOrWhiteSpace(PathName))
                    throw new ArgumentOutOfRangeException("AliSentinelConfig Resource 配置错误");
                return $"POST:/v1.0/invoke/{ServiceName.ToLower()}/method/{PathName.ToLower()}";
            }
        }
        /// <summary>
        /// 根据metadata获取原始路径对应的服务名和路径
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public void SetServiceAndPathName(string key, JObject item)
        {
            if (item[key] == null && item[key.ToLower()] == null)
                throw new ArgumentOutOfRangeException("FullPatch配置错误");
            var val = (item[key] ?? item[key.ToLower()]).ToString();
            this.ServiceName = val.Replace("POST:/v1.0/invoke/", "").Replace("method", "").Split("//")[0];
            this.PathName = val.Replace("POST:/v1.0/invoke/", "").Replace("method", "").Split("//")[1];
        }
    }
}
