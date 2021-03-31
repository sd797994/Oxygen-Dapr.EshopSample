using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TradeLogRepository : RepositoryBase<EfDbContext, Domain.Entities.TradeLog, TradeLog>, Domain.Repository.ITradeLogRepository
    {
        private readonly EfDbContext context;
        public TradeLogRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
