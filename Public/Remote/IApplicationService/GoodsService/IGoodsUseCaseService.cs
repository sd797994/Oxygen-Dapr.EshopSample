using IApplicationService.GoodsService.Dtos.Input;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "goodsusecase", "商品服务")]
    public interface IGoodsUseCaseService
    {
        [RemoteFunc(funcDescription: "创建商品")]
        Task<ApiResult> CreateGoods(GoodsCreateDto input);

        [RemoteFunc(funcDescription: "更新商品基础信息")]
        Task<ApiResult> UpdateGoods(GoodsUpdateDto input);

        [RemoteFunc(funcDescription: "删除商品")]
        Task<ApiResult> DeleteGoods(GoodsDeleteDto input);

        [RemoteFunc(funcDescription: "上下架商品")]
        Task<ApiResult> UpOrDownShelfGoods(UpOrDownShelfGoodsDto input);

        [RemoteFunc(funcDescription: "更新商品库存")]
        Task<ApiResult> UpdateGoodsStock(DeductionStockDto input);

        [RemoteFunc(funcDescription: "修改商品库存")]
        Task<ApiResult> DeductionGoodsStock(DeductionStockDto input);
    }
}
