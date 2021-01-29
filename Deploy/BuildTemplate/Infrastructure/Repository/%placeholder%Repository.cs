using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class %placeholder%Repository : RepositoryBase<EfDbContext, Domain.Entities.%placeholder%, %placeholder%>, Domain.Repository.I%placeholder%Repository
    {
        private readonly EfDbContext context;
        public %placeholder%Repository(EfDbContext context) : base(context) { this.context = context; }

    }
}
