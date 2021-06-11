using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.SentinelConfig.Enums
{
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
}
