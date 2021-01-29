using ApplicationService.Dtos;
using Autofac;
using Domain;
using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Object;
using Oxygen.Mesh.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsActorService: BaseActorService<GoodsActor>, IGoodsActorService
    {
        private readonly IGoodsRepository repository;
        private readonly IUnitofWork unitofWork;
        public GoodsActorService(IGoodsRepository repository, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
        }
        public async Task<ApiResult> UpdateGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok("商品库存更新成功!").RunAsync(async () =>
            {
                if (ActorData == null)
                    ActorData = (await repository.GetAsync(input.GoodsId)).CopyTo<Goods, GoodsActor>();
                if (ActorData == null)
                    throw new ApplicationServiceException("没有查询到该商品!");
                ActorData.ChangeStock(input.DeductionStock);
            });
        }

        public async Task<ApiResult> DeductionGoodsStock(DeductionStockDto input)
        {
            if (ActorData == null)
                ActorData = (await repository.GetAsync(input.GoodsId)).CopyTo<Goods, GoodsActor>();
            ActorData.DeductionStock(input.DeductionStock);
            return ApiResult.Ok("商品库存减扣成功");
        }
        public async Task<ApiResult> UnDeductionGoodsStock(DeductionStockDto input)
        {
            if (ActorData == null)
                ActorData = (await repository.GetAsync(input.GoodsId)).CopyTo<Goods, GoodsActor>();
            ActorData.UnDeductionStock(input.DeductionStock);
            return ApiResult.Ok("商品库存回滚成功");
        }

        public override async Task SaveData(GoodsActor model, ILifetimeScope scope)
        {
            var goods = await repository.GetAsync(model.Id);
            if (goods != null)
                goods.ChangeStock(model.Stock);
            repository.Update(goods);
            await unitofWork.CommitAsync();
            await Task.CompletedTask;
        }
    }
}
