using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// sentinel组件配置基类
    /// </summary>
    public abstract class AliSentinelConfig
    {
        public string ServiceName { get; set; }
        public string PathName { get; set; }
        public string GetFullPatch()
        {
            if (string.IsNullOrWhiteSpace(ServiceName) || string.IsNullOrWhiteSpace(PathName))
                throw new ArgumentOutOfRangeException("FullPatch配置错误");
            return $"POST:/v1.0/invoke/{ServiceName}/method/{PathName}";
        }

    }
    /// <summary>
    /// 限流配置
    /// </summary>
    public class AliSentinelConfigFlowRule : AliSentinelConfig
    {
        /// <summary>
        /// 限流策略
        /// </summary>
        public ControlBehavior ControlBehavior { get; set; }
        /// <summary>
        /// QPS
        /// </summary>
        public int Threshold { get; set; }
        /// <summary>
        /// 限流类型为队列最大等待时间
        /// </summary>
        public int? MaxQueueingTimeMs { get; set; }
    }
    /// <summary>
    /// 流控策略
    /// </summary>
    public enum ControlBehavior
    {
        /// <summary>
        /// 直接拒绝
        /// </summary>
        Reject = 0,
        /// <summary>
        /// 队列等待
        /// </summary>
        Throttling = 1
    }
    /// <summary>
    /// 熔断配置
    /// </summary>
    public class AliSentinelConfigCircuitBreakingRule : AliSentinelConfig
    {
        /// <summary>
        /// 熔断策略
        /// </summary>
        public Strategy Strategy { get; set; }
        /// <summary>
        /// 重试时间
        /// </summary>
        public int RetryTimeoutMs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MinRequestAmount { get; set; }
        /// <summary>
        /// 慢调用比例阈值
        /// </summary>
        public int? Threshold { get; set; }
    }

    /// <summary>
    /// 熔断策略
    /// </summary>
    public enum Strategy
    {
        /// <summary>
        /// 慢调用比例
        /// </summary>
        SlowRequestRatio = 0,
        /// <summary>
        /// 错误比例
        /// </summary>
        ErrorRatio = 1,
        /// <summary>
        /// 错误数
        /// </summary>
        ErrorCount = 2
    }
}
