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
    public interface GoodsUseCaseService
    {
        [RemoteFunc(funcDescription: "创建商品")]
        Task<ApiResult> CreateGoods(GoodsCreateDto input);

        [RemoteFunc(funcDescription: "更新商品基础信息")]
        Task<ApiResult> UpdateGoods(GoodsUpdateDto input);

        [RemoteFunc(funcDescription: "删除商品")]
        Task<ApiResult> DeleteGoods(GoodsDeleteDto input);

        [RemoteFunc(funcDescription: "上下架商品")]
        Task<ApiResult> UpOrDownShelfGoods(UpOrDownShelfGoodsDto input);
    }

}
