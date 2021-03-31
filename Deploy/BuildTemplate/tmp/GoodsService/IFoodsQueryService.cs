using Oxygen.Client.ServerSymbol;
using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("", "", "")]
    public interface IFoodsQueryService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> GetFoodsList(PageQueryInputBase input);
    }
}
