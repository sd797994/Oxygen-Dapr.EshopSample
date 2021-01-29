using Domain.Entities;
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
        public async Task<bool> IsSatisfiedBy(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
