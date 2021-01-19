using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        new Task Add(Domain.Role t);
        new Task Update(Domain.Role t);
    }
}
