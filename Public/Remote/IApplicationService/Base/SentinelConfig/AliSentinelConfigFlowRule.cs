using IApplicationService.Base.SentinelConfig.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.SentinelConfig
{
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
        /// <summary>
        /// 统计周期
        /// </summary>
        public int StatIntervalInMs { get; set; } = 1000;
    }
}
