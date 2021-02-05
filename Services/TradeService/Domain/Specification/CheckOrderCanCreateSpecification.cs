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
    public class CheckOrderCanCreateSpecification : ISpecification<Order>
    {
        private readonly IOrderRepository repository;
        public CheckOrderCanCreateSpecification(IOrderRepository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> IsSatisfiedBy(Order entity)
        {
            if (await repository.AnyAsync(x => x.OrderNo == entity.OrderNo))
                throw new DomainException("订单号重复!");
            return true;
        }
    }
}
