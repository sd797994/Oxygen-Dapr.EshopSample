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
    public class CheckTradeLogCanCreateSpecification : ISpecification<TradeLog>
    {
        private readonly IOrderRepository orderRepository;
        public CheckTradeLogCanCreateSpecification(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<bool> IsSatisfiedBy(TradeLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
