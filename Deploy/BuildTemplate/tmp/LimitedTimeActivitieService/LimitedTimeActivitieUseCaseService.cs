using Oxygen.Client.ServerSymbol;
using IApplicationService.LimitedTimeActivitieService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.LimitedTimeActivitieService
{
    [RemoteService("", "", "")]
    public interface LimitedTimeActivitieUseCaseService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> CreateLimitedTimeActivitie(LimitedTimeActivitieCreateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> UpdateLimitedTimeActivitie(LimitedTimeActivitieUpdateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> DeleteLimitedTimeActivitie(LimitedTimeActivitieDeleteDto input);
    }
}
