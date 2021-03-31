using ApplicationService.Dtos;
using Autofac;
using Domain;
using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using Infrastructure.EfDataAccess;
using Infrastructure.Elasticsearch;
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
        private readonly ILocalEventBus localEventBus;
        public GoodsActorService(IGoodsRepository repository, IUnitofWork unitofWork, ILocalEventBus localEventBus)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
            this.localEventBus = localEventBus;
        }
        public async Task<ApiResult> UpdateGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存更新成功!").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.ChangeStock(input.DeductionStock);
            });
        }

        public async Task<ApiResult> DeductionGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存减扣成功").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.DeductionStock(input.DeductionStock);
            });
        }
        public async Task<ApiResult> UnDeductionGoodsStock(DeductionStockDto input)
        {
            return await ApiResult.Ok(true, "商品库存回滚成功").RunAsync(async () =>
            {
                await SetActorDataIfNotExists(input.GoodsId);
                ActorData.UnDeductionStock(input.DeductionStock);
            });
        }
        async Task SetActorDataIfNotExists(Guid id)
        {
            if (ActorData == null)
            {
                Goods goods = default;
                try
                {
                    goods = await repository.GetAsync(id);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"商品对象获取失败,失败原因：{e.GetBaseException()?.Message ?? e.Message}");
                }
                finally
                {
                    if (goods == null)
                        throw new ApplicationServiceException("没有找到该商品!");
                }
                ActorData = goods.CopyTo<Goods, GoodsActor>();
            }
        }
        public override async Task SaveData(GoodsActor model, ILifetimeScope scope)
        {
            var goods = await repository.GetAsync(model.Id);
            if (goods != null)
                goods.ChangeStock(model.Stock);
            repository.Update(goods);
            await localEventBus.SendEvent(EventTopicDictionary.Goods.Loc_WriteToElasticsearch, goods);
            await unitofWork.CommitAsync();
            await Task.CompletedTask;
        }
    }
}
