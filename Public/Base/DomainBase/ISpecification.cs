using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase
{
    public interface ISpecificationn<TEntity>
    {
        Task<bool> IsSatisfiedBy(TEntity entity);
    }
}
