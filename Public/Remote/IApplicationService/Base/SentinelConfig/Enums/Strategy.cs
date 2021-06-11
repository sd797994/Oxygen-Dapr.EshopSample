using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.SentinelConfig.Enums
{
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
