using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class PermissionRepository : RepositoryBase<EfDbContext, Domain.Permission, Permission>, Domain.Repository.IPermissionRepository
    {
        private readonly EfDbContext context;
        public PermissionRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
