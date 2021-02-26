using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService
{
    [RemoteService("", "", "")]
    public interface IEventHandleErrorInfoQueryService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> GetEventHandleErrorInfoList(PageQueryInputBase input);
    }
}
