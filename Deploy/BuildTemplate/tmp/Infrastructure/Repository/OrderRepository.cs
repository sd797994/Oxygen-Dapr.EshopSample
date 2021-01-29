using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class OrderRepository : RepositoryBase<EfDbContext, Domain.Order, Order>, Domain.Repository.IOrderRepository
    {
        private readonly EfDbContext context;
        public OrderRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
