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
    public class CheckGoodsCategoryCanRemoveSpecification : ISpecification<GoodsCategory>
    {
        private readonly IGoodsRepository goodsRepository;
        public CheckGoodsCategoryCanRemoveSpecification(IGoodsRepository goodsRepository)
        {
            this.goodsRepository = goodsRepository;
        }
        public async Task<bool> IsSatisfiedBy(GoodsCategory entity)
        {
            if (await goodsRepository.AnyAsync(x => x.CategoryId == entity.Id))
                throw new DomainException($"当前商品分类下包含商品，无法删除");
            else
                return true;
        }
    }
}
