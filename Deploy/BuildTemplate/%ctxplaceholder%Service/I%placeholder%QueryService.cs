using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.%ctxplaceholder%Service
{
    [RemoteService("", "", "")]
    public interface I%placeholder%QueryService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> Get%placeholder%List(PageQueryInputBase input);
    }
}
