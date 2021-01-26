using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.LimitedTimeActivitieService
{
    [RemoteService("", "", "")]
    public interface LimitedTimeActivitieQueryService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> GetLimitedTimeActivitieList(PageQueryInputBase input);
    }
}
