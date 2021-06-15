using IApplicationService.Base.SentinelConfig.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.SentinelConfig
{
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
        /// 慢调用临界值单位为 ms
        /// </summary>
        public int? MaxAllowedRtMs { get; set; }
        /// <summary>
        /// 比例阈值
        /// </summary>
        public decimal Threshold { get; set; }
        /// <summary>
        /// 统计周期
        /// </summary>
        public int StatIntervalMs { get; set; } = 1000;
        
    }
}
