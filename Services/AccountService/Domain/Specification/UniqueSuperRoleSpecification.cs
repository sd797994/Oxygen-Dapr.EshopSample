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
    public class UniqueSuperRoleSpecification : ISpecification<Role>
    {
        private readonly IRoleRepository roleRepository;

        public UniqueSuperRoleSpecification(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> IsSatisfiedBy(Role _)
        {
            if (await roleRepository.AnyAsync())
                throw new DomainException("无法进行RBAC初始化!");
            return true;
        }
    }
}
