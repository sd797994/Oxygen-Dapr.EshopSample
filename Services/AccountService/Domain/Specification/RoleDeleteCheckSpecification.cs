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
    public class RoleDeleteCheckSpecification : ISpecification<Role>
    {
        private readonly IRoleRepository rolerepository;
        public RoleDeleteCheckSpecification(IRoleRepository rolerepository)
        {
            this.rolerepository = rolerepository;
        }

        public async Task<bool> IsSatisfiedBy(Role entity)
        {
            if (await rolerepository.RoleRelationUser(entity.Id))
                throw new DomainException("当前角色包含用户关联，不能删除");
            return true;
        }
    }
}
