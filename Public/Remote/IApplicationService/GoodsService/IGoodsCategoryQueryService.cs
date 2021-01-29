using IApplicationService.Base.AppQuery;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsCategoryService
{
    [RemoteService("goodsservice", "categoryquery","商品分类服务")]
    public interface IGoodsCategoryQueryService
    {

        [RemoteFunc(funcDescription: "获取商品分类列表")]
        Task<ApiResult> GetCategoryList(PageQueryInputBase input);
        [RemoteFunc(funcDescription: "获取全部商品分类")]
        Task<ApiResult> GetAllCategoryList();
    }
}
