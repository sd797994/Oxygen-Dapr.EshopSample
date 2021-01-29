using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Domain.Entities;

namespace Domain.Specification
{
    public class LimitedTimeActivitieValidityCheckSpecification : ISpecification<LimitedTimeActivitie>
    {
        private readonly IGoodsRepository goodsRepository;
        public LimitedTimeActivitieValidityCheckSpecification(IGoodsRepository goodsRepository)
        {
            this.goodsRepository = goodsRepository;
        }
        public async Task<bool> IsSatisfiedBy(LimitedTimeActivitie entity)
        {
            //检查活动商品有效性
            var goods = await goodsRepository.GetAsync(entity.GoodsId);
            if (goods == null)
                throw new DomainException("所选商品无效!");
            if (goods.Price <= entity.ActivitiePrice)
                throw new DomainException("活动促销价应低于商品原价!");
            return true;
        }
    }
}
