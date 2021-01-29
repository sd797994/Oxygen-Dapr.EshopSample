using Domain.Entities;
using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specification
{
    public class GoodsValidityCheckSpecification : ISpecification<Goods>
    {
        private readonly IGoodsRepository goodsRepository;
        private readonly IGoodsCategoryRepository goodsCategoryRepository;
        public GoodsValidityCheckSpecification(IGoodsRepository goodsRepository, IGoodsCategoryRepository goodsCategoryRepository)
        {
            this.goodsRepository = goodsRepository;
            this.goodsCategoryRepository = goodsCategoryRepository;
        }
        public async Task<bool> IsSatisfiedBy(Goods entity)
        {
            if (await goodsRepository.AnyAsync(x => x.Id != entity.Id && x.GoodsName == entity.GoodsName))
                throw new DomainException("产品名称重复!");
            if (!await goodsCategoryRepository.AnyAsync(entity.CategoryId))
                throw new DomainException("没查询到对应的产品分类!");
            return true;
        }
    }
    
}
