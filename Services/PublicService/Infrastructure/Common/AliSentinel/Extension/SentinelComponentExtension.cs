using IApplicationService.Base.SentinelConfig;
using IApplicationService.Base.SentinelConfig.Enums;
using k8s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.AliSentinel.Extension
{
    public static class SentinelComponentExtension
    {
        /// <summary>
        /// 将规则模型转化为metadata
        /// </summary>
        /// <param name="sentinelComponent"></param>
        public static void SetMetaData(this SentinelComponent sentinelComponent)
        {
            foreach (var item in sentinelComponent.spec.metadata)
            {
                if (item["name"] == SentinelComponentBaseConfig.flowrulename)
                {
                    if (sentinelComponent.FlowRules != null)
                    {
                        item["value"] = JsonConvert.SerializeObject(sentinelComponent.FlowRules);
                    }
                    else
                    {
                        item["value"] = "[]";
                    }
                }
                else if(item["name"] == SentinelComponentBaseConfig.circuitbreakerrulename)
                {
                    if (sentinelComponent.BreakingRules != null)
                    {
                        item["value"] = JsonConvert.SerializeObject(sentinelComponent.BreakingRules);
                    }
                    else
                    {
                        item["value"] = "[]";
                    }
                }
            }
        }
        /// <summary>
        /// 初始化SentinelComponent并将metadata转化为规则模型
        /// </summary>
        /// <param name="sentinelComponent"></param>
        /// <param name="kubernetes"></param>
        public static async Task Create(this SentinelComponent sentinelComponent, Kubernetes kubernetes)
        {
            try
            {
                var remoteobj = await kubernetes.GetNamespacedCustomObjectWithHttpMessagesAsync(SentinelComponentBaseConfig.Group, SentinelComponentBaseConfig.Version, SentinelComponentBaseConfig.NamespaceParameter, SentinelComponentBaseConfig.Plural, SentinelComponentBaseConfig.ComponentName);
                if (remoteobj.Response.IsSuccessStatusCode)
                {
                    sentinelComponent.spec = JsonConvert.DeserializeObject<SentinelComponent>(await remoteobj.Response.Content.ReadAsStringAsync()).spec;
                    foreach (var metadata in sentinelComponent.spec.metadata)
                    {
                        if (metadata["name"] == SentinelComponentBaseConfig.flowrulename)
                        {
                            foreach (var item in JsonConvert.DeserializeObject<List<JObject>>(metadata["value"]))
                            {
                                var oldrule = new AliSentinelConfigFlowRule();
                                oldrule.SetServiceAndPathName(nameof(AliSentinelConfig.Resource), item);
                                oldrule.ControlBehavior = setValue(item, nameof(AliSentinelConfigFlowRule.ControlBehavior), ControlBehavior.Reject, (x) => (ControlBehavior)Enum.Parse(typeof(ControlBehavior), x));
                                oldrule.MaxQueueingTimeMs = setValue(item, nameof(AliSentinelConfigFlowRule.MaxQueueingTimeMs), 1000, x => setIntValue(x));
                                oldrule.Threshold = setValue(item, nameof(AliSentinelConfigFlowRule.Threshold), 1000, x => setIntValue(x));
                                sentinelComponent.FlowRules.Add(oldrule);
                            }
                        }
                        else if (metadata["name"] == SentinelComponentBaseConfig.circuitbreakerrulename)
                        {
                            foreach (var item in JsonConvert.DeserializeObject<List<JObject>>(metadata["value"]))
                            {
                                var oldrule = new AliSentinelConfigCircuitBreakingRule();
                                oldrule.SetServiceAndPathName(nameof(AliSentinelConfig.Resource), item);
                                oldrule.Threshold = setValue(item, nameof(AliSentinelConfigCircuitBreakingRule.Threshold), 1000, x => setIntValue(x));
                                oldrule.MaxAllowedRtMs = setValue(item, nameof(AliSentinelConfigCircuitBreakingRule.MaxAllowedRtMs), 1000, x => setIntValue(x));
                                oldrule.RetryTimeoutMs = setValue(item, nameof(AliSentinelConfigCircuitBreakingRule.RetryTimeoutMs), 1000, x => setIntValue(x));
                                oldrule.Strategy = setValue(item, nameof(AliSentinelConfigCircuitBreakingRule.Strategy), Strategy.ErrorCount, x => setEnumValue<Strategy>(x));
                                sentinelComponent.BreakingRules.Add(oldrule);
                            }
                        }
                    }
                    //私有方法用于赋值
                    T setValue<T>(JObject item, string key, T defaultval, Func<string, T> func)
                    {
                        if (item[key.ToLower()] == null && item[key] == null)
                            return defaultval;
                        else
                            return func((item[key] ?? item[key.ToLower()]).ToString());
                    }
                    int setIntValue(string s) => string.IsNullOrEmpty(s) ? 0 : int.TryParse(s, out int result) ? result : 0;
                    EnumType setEnumValue<EnumType>(string s) => string.IsNullOrEmpty(s) ? default : Enum.TryParse(typeof(EnumType), s, out object result) ? default : default;
                }
                else
                    throw new NullReferenceException($"在{SentinelComponentBaseConfig.NamespaceParameter}下未找到{SentinelComponentBaseConfig.ComponentName} Component");
            }
            catch(Exception)
            {
                throw new NullReferenceException($"在{SentinelComponentBaseConfig.NamespaceParameter}下未找到{SentinelComponentBaseConfig.ComponentName} Component 或加载Component失败");
            }
        }
    }
}