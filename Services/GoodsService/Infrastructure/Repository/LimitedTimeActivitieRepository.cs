using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class LimitedTimeActivitieRepository : RepositoryBase<EfDbContext, Domain.Entities.LimitedTimeActivitie, LimitedTimeActivitie>, Domain.Repository.ILimitedTimeActivitieRepository
    {
        private readonly EfDbContext context;
        public LimitedTimeActivitieRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
