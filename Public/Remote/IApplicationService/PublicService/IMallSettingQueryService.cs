using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService
{
    [RemoteService("publicservice", "mallsettingquery", "公共服务")]
    public interface IMallSettingQueryService
    {
        [RemoteFunc(funcDescription: "获取商城配置")]
        Task<ApiResult> GetMallSetting();
    }
}
