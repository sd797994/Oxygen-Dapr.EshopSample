using Oxygen.Client.ServerSymbol;
using IApplicationService.PublicService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService
{
    [RemoteService("", "", "")]
    public interface IEventHandleErrorInfoUseCaseService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> CreateEventHandleErrorInfo(EventHandleErrorInfoCreateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> UpdateEventHandleErrorInfo(EventHandleErrorInfoUpdateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> DeleteEventHandleErrorInfo(EventHandleErrorInfoDeleteDto input);
    }
}
