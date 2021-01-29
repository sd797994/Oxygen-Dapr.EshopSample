using Oxygen.Client.ServerSymbol;
using IApplicationService.LimitedTimeActivitieService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IApplicationService.GoodsService.Dtos.Input;

namespace IApplicationService.LimitedTimeActivitieService
{
    [RemoteService("goodsservice", "activitiusecase", "限时活动服务")]
    public interface ILimitedTimeActivitieUseCaseService
    {
        [RemoteFunc(funcDescription: "创建限时活动")]
        Task<ApiResult> CreateLimitedTimeActivitie(LimitedTimeActivitieCreateDto input);
		
        [RemoteFunc(funcDescription: "更新限时活动信息")]
        Task<ApiResult> UpdateLimitedTimeActivitie(LimitedTimeActivitieUpdateDto input);
		
        [RemoteFunc(funcDescription: "删除限时活动")]
        Task<ApiResult> DeleteLimitedTimeActivitie(LimitedTimeActivitieDeleteDto input);

        [RemoteFunc(funcDescription: "限时活动上下架")]
        Task<ApiResult> UpOrDownShelfActivitie(UpOrDownShelfActivitieDto input);
    }
}
