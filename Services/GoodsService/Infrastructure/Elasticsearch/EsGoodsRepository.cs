using Domain.Entities;
using Infrastructure.EfDataAccess;
using InfrastructureBase.Data.Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Elasticsearch
{
    public class EsGoodsRepository: IEsGoodsRepository
    {
        private readonly IElasticSearchRepository<EsGoodsDto> elasticRepo;
        private readonly EfDbContext efDbContext;
        public EsGoodsRepository(IElasticSearchRepository<EsGoodsDto> elasticRepo, EfDbContext efDbContext)
        {
            this.elasticRepo = elasticRepo;
            this.efDbContext = efDbContext;
        }
        public async Task WriteToElasticsearch(Goods goods)
        {
            var s = new EsGoodsDto()
            {
                CategoryId = goods.CategoryId,
                CategoryName = efDbContext.GoodsCategory.Find(goods.CategoryId).CategoryName,
                Id = goods.Id,
                Name = goods.GoodsName,
                Price = goods.Price,
                OldPrice = goods.Price,
                SellCount = 0,
                Icon = goods.GoodsImage,
            };
            await elasticRepo.GetRepo("goods").SaveData(s);
        }
        public async Task RemoveToElasticsearch(Goods goods)
        {
            await elasticRepo.GetRepo("goods").RemoveData(new EsGoodsDto() { Id = goods.Id });
        }
    }
}
