using IApplicationService.GoodsService.Dtos.Input;
using Oxygen.Client.ServerSymbol;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "goodsactor", "商品服务")]
    public interface IGoodsActorService : IActorService
    {
        [RemoteFunc(FuncType.Actor, funcDescription: "更新商品库存")]
        Task<ApiResult> UpdateGoodsStock(DeductionStockDto input);

        [RemoteFunc(FuncType.Actor, funcDescription: "修改商品库存")]
        Task<ApiResult> DeductionGoodsStock(DeductionStockDto input);
    }
}
