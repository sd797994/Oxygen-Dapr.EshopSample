using Oxygen.Client.ServerSymbol;
using IApplicationService.%ctxplaceholder%Service.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.%ctxplaceholder%Service
{
    [RemoteService("", "", "")]
    public interface I%placeholder%UseCaseService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> Create%placeholder%(%placeholder%CreateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> Update%placeholder%(%placeholder%UpdateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> Delete%placeholder%(%placeholder%DeleteDto input);
    }
}
