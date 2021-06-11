using IApplicationService.Base.SentinelConfig;
using k8s;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// k8s SentinelComponent原始类型
    /// </summary>
    public class SentinelComponent
    {
        public class Spec
        {
            public Dictionary<string, string>[] metadata { get; set; }
        }
        public List<AliSentinelConfigFlowRule> FlowRules { get; set; } = new List<AliSentinelConfigFlowRule>();
        public List<AliSentinelConfigCircuitBreakingRule> BreakingRules { get; set; } = new List<AliSentinelConfigCircuitBreakingRule>();
        public Spec spec { get; set; }
    }
}
