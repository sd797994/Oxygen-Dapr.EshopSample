using Domain;
using Domain.Entities;
using Domain.Repository;
using Domain.Specification;
using IApplicationService;
using IApplicationService.GoodsCategoryService;
using IApplicationService.GoodsService.Dtos.Input;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class GoodsCategoryUseCaseService : IGoodsCategoryUseCaseService
    {
        private readonly IGoodsCategoryRepository repository;
        private readonly IGoodsRepository goodsRepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        public GoodsCategoryUseCaseService(IGoodsCategoryRepository repository, IGoodsRepository goodsRepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.goodsRepository = goodsRepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }
        [AuthenticationFilter]
        public async Task<ApiResult> CreateCategory(CategoryCreateDto input)
        {
            var goodsCategory = new GoodsCategory();
            goodsCategory.CreateOrUpdate(input.CategoryName, input.Sort);
            repository.Add(goodsCategory);
            if (await new UniqueGoodsCategoryNameSpecification(repository).IsSatisfiedBy(goodsCategory))
                await unitofWork.CommitAsync();
            return ApiResult.Ok("商品分类创建成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> DeleteCategory(CategoryDeleteDto input)
        {
            var goodsCategory = await repository.GetAsync(input.Id);
            if (goodsCategory == null)
                throw new ApplicationServiceException("没有找到该商品分类!");
            repository.Delete(goodsCategory);
            if (await new CheckGoodsCategoryCanRemoveSpecification(goodsRepository).IsSatisfiedBy(goodsCategory))
                await unitofWork.CommitAsync();
            return ApiResult.Ok("商品分类删除成功");
        }
        [AuthenticationFilter]
        public async Task<ApiResult> UpdateCategory(CategoryUpdateDto input)
        {
            var goodsCategory = await repository.GetAsync(input.Id);
            if (goodsCategory == null)
                throw new ApplicationServiceException("没有找到该商品分类!");
            goodsCategory.CreateOrUpdate(input.CategoryName, input.Sort);
            repository.Update(goodsCategory);
            if (await new UniqueGoodsCategoryNameSpecification(repository).IsSatisfiedBy(goodsCategory))
                await unitofWork.CommitAsync();
            return ApiResult.Ok("商品分类更新成功");
        }
    }
}
