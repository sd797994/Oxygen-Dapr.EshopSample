using IApplicationService.Base.AppQuery;
using IApplicationService.GoodsService.Dtos.Input;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService
{
    [RemoteService("goodsservice", "goodsquery","商品服务")]
    public interface IGoodsQueryService
    {
        [RemoteFunc(funcDescription: "获取商品列表")]
        Task<ApiResult> GetGoodsList(PageQueryInputBase input);
        [RemoteFunc(funcDescription: "获取商品列表")]
        Task<ApiResult> GetGoodsListByIds(GetGoodsListByIdsDto input);

        [RemoteFunc(funcDescription: "搜索商品列表")]
        Task<ApiResult> GetGoodslistByGoodsName(GetGoodslistByGoodsNameDto input);
    }
}
