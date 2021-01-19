using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specification
{
    public class RoleValidityCheckSpecification : ISpecificationn<Account>
    {
        private readonly IRoleRepository roleRepository;

        public RoleValidityCheckSpecification(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<bool> IsSatisfiedBy(Account entity)
        {
            if (entity.Roles.Any())
            {
                var validRoles = new List<Guid>();
                await foreach (var role in roleRepository.GetManyAsync(entity.Roles.ToArray()))
                {
                    validRoles.Add(role.Id);
                }
                if (entity.Roles.Except(validRoles).Any())
                    throw new DomainException("部分所选角色无效!");
            }
            return true;
        }
    }
}
