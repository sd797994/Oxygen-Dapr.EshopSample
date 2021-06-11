using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.Base.SentinelConfig;
using IApplicationService.PublicService.Dtos.Input;

namespace IApplicationService.Base
{
    [RemoteService("publicservice", "sentinelconfig", "sentinel配置")]
    public interface IAliSentinelConfigService
    {
        [RemoteFunc(funcDescription: "获取配置")]
        Task<ApiResult> Get();

        [RemoteFunc(funcDescription: "保存配置")]
        Task<ApiResult> Save(SentinelConfigList config);

        [RemoteFunc(funcDescription: "获取其他基础数据")]
        Task<ApiResult> GetCommonData();
    }
}
