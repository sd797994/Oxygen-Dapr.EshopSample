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
        public new async Task Add(Domain.Role t)
        {
            var role = t.CopyTo<Domain.Role, Role>();
            context.Role.Add(role);
            if (role.Permissions.Any())
            {
                role.Permissions = await context.Permission.Where(x => role.Permissions.Contains(x.Id) && x.FatherId != Guid.Empty).Select(x => x.Id).ToListAsync();
                context.RolePermission.AddRange(role.Permissions.Select(x => new RolePermission() { RoleId = role.Id, PermissionId = x }));
            }
        }
        public new async Task Update(Domain.Role t)
        {
            var role = t.CopyTo<Domain.Role, Role>();
            context.Role.Update(role);
            if (role.Permissions.Any())
            {
                var oldPermissions = await context.RolePermission.Where(x => x.RoleId == role.Id).ToListAsync();
                context.RolePermission.RemoveRange(oldPermissions);
                context.RolePermission.AddRange(role.Permissions.Select(x => new RolePermission() { RoleId = role.Id, PermissionId = x }));
            }
        }
    }
}
