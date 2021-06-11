using IApplicationService.Base.SentinelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService.Dtos.Input
{
    public class SentinelConfigList
    {
        public List<AliSentinelConfigFlowRule> FlowRules { get; set; }
        public List<AliSentinelConfigCircuitBreakingRule> BreakingRules { get; set; }
    }
}
