using Domain.Repository;
using IApplicationService;
using IApplicationService.GoodsService.Dtos.Input;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using Domain;
using InfrastructureBase.AuthBase;
using Domain.Specification;
using InfrastructureBase;
using Oxygen.Mesh.Dapr;
using ApplicationService.Dtos;
using Autofac;
using InfrastructureBase.Object;
using System;
using IApplicationService.GoodsService;
using Domain.Entities;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Client.ServerSymbol;
using Oxygen.Client.ServerSymbol.Actors;
using Oxygen.Mesh.Dapr.Model;

namespace ApplicationService
{
    public class GoodsUseCaseService : BaseActorService<GoodsActor>, IGoodsUseCaseService
    {
        private readonly IGoodsRepository repository;
        private readonly IGoodsCategoryRepository goodsCategoryRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IGoodsActorService actorService;
        private readonly IServiceProxyFactory serviceProxyFactory;
        public GoodsUseCaseService(IGoodsRepository repository, IGoodsCategoryRepository goodsCategoryRepository, IServiceProxyFactory serviceProxyFactory, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.goodsCategoryRepository = goodsCategoryRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.serviceProxyFactory = serviceProxyFactory;
            this.actorService = serviceProxyFactory.CreateActorProxy<IGoodsActorService>();
        }
        [AuthenticationFilter]
        public async Task<ApiResult> CreateGoods(GoodsCreateDto input)
        {
            var goods = new Goods();
            goods.CreateOrUpdateGoods(input.GoodsName, input.GoodsImage, input.Price, input.CategoryId);
            goods.UpOrDownShelf(false);
            goods.ChangeStock(input.Stock);
            repository.Add(goods);
            if (await new GoodsValidityCheckSpecification(repository, goodsCategoryRepository).IsSatisfiedBy(goods))
                await unitofWork.CommitAsync();
            return ApiResult.Ok("商品创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateGoods(GoodsUpdateDto input)
        {
            var goods = await repository.GetAsync(input.Id);
            if (goods == null)
                throw new ApplicationServiceException("没有查询到该商品!");
            goods.CreateOrUpdateGoods(input.GoodsName, input.GoodsImage, input.Price, input.CategoryId);
            repository.Update(goods);
            if (await new GoodsValidityCheckSpecification(repository, goodsCategoryRepository).IsSatisfiedBy(goods))
                await unitofWork.CommitAsync();
            return ApiResult.Ok("商品更新成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> UpOrDownShelfGoods(UpOrDownShelfGoodsDto input)
        {
            var goods = await repository.GetAsync(input.Id);
            if (goods == null)
                throw new ApplicationServiceException("没有查询到该商品!");
            goods.UpOrDownShelf(input.ShelfState);
            repository.Update(goods);
            await unitofWork.CommitAsync();
            return ApiResult.Ok("商品上架/下架成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> DeleteGoods(GoodsDeleteDto input)
        {
            var entity = await repository.GetAsync(input.Id);
            if (entity == null)
                throw new ApplicationServiceException("没有查询到该商品!");
            repository.Delete(entity);
            await unitofWork.CommitAsync();
            return ApiResult.Ok("商品删除成功");
        }

        [AuthenticationFilter]
        public async Task<ApiResult> UpdateGoodsStock(DeductionStockDto input)
        {
            input.ActorId = input.GoodsId.ToString();
            var result =  await actorService.UpdateGoodsStock(input);
            return result;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> DeductionGoodsStock(DeductionStockDto input)
        {
            input.ActorId = input.GoodsId.ToString();
            return await actorService.DeductionGoodsStock(input);
        }

        public override async Task SaveData(GoodsActor model, ILifetimeScope scope)
        {
            var goods = await repository.GetAsync(model.Id);
            if (goods != null)
                goods.ChangeStock(model.Stock);
            await unitofWork.CommitAsync();
            await Task.CompletedTask;
        }
    }
}
