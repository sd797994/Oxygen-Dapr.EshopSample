using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class LogisticsRepository : RepositoryBase<EfDbContext, Domain.Entities.Logistics, Logistics>, Domain.Repository.ILogisticsRepository
    {
        private readonly EfDbContext context;
        public LogisticsRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
