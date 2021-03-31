using Oxygen.Client.ServerSymbol;
using IApplicationService.GoodsService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("", "", "")]
    public interface IFoodsUseCaseService
    {
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> CreateFoods(FoodsCreateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> UpdateFoods(FoodsUpdateDto input);
		
        [RemoteFunc(funcDescription: "")]
        Task<ApiResult> DeleteFoods(FoodsDeleteDto input);
    }
}
