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
    public class LogisticsDeliverCheckSpecification : ISpecification<Logistics>
    {
        private readonly ILogisticsRepository logisticsRepository;
        public LogisticsDeliverCheckSpecification(ILogisticsRepository logisticsRepository)
        {
            this.logisticsRepository = logisticsRepository;
        }
        public async Task<bool> IsSatisfiedBy(Logistics entity)
        {
            if (await logisticsRepository.AnyAsync(x => x.OrderId == entity.OrderId))
                throw new DomainException("当前订单已包含物流信息，无法再次发货!");
            return true;
        }
    }
}
