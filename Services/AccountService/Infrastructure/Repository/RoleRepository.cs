using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RoleRepository : RepositoryBase<EfDbContext, Domain.Role, Role>, Domain.Repository.IRoleRepository
    {
        private readonly EfDbContext context;
        public RoleRepository(EfDbContext context) : base(context) { this.context = context; }
        public override void Add(Domain.Role t)
        {
            var role = t.CopyTo<Domain.Role, Role>();
            context.Role.Add(role);
            handleRolePermission(role);
        }
        public override void Update(Domain.Role t)
        {
            var role = t.CopyTo<Domain.Role, Role>();
            context.Role.Update(role);
            handleRolePermission(role);
        }
        public override void Delete(Domain.Role t)
        {
            var role = t.CopyTo<Domain.Role, Role>();
            context.Role.Remove(role);
            context.RolePermission.RemoveRange(context.RolePermission.Where(x => x.RoleId == role.Id));
        }
        void handleRolePermission(Role role)
        {
            context.RolePermission.RemoveRange(context.RolePermission.Where(x => x.RoleId == role.Id));
            if (role.Permissions != null && role.Permissions.Any())
            {
                context.RolePermission.AddRange(context.Permission.Where(x => x.FatherId != Guid.Empty && role.Permissions.Contains(x.Id)).Select(x => new RolePermission() { RoleId = role.Id, PermissionId = x.Id }));
            }
        }

        public async Task<bool> RoleRelationUser(Guid roleId)
        {
            return await context.UserRole.AnyAsync(x => x.RoleId == roleId);
        }
    }
}
