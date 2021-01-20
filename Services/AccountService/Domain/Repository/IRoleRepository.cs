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
        Task<bool> RoleRelationUser(Guid roleId);
    }
}
