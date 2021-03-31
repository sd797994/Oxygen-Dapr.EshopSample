using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class OrderRepository : RepositoryBase<EfDbContext, Domain.Entities.Order, Order>, Domain.Repository.IOrderRepository
    {
        private readonly EfDbContext context;
        public OrderRepository(EfDbContext context) : base(context) { this.context = context; }

        public override void Add(Domain.Entities.Order t)
        {
            base.Add(t);
            context.OrderItem.AddRange(t.OrderItems.CopyTo<Domain.Entities.OrderItem, OrderItem>());
        }
        public override async Task<Domain.Entities.Order> GetAsync(object key)
        {
            var result = await base.GetAsync(key);
            result.OrderItems = (await context.OrderItem.Where(x => x.OrderId == result.Id).ToListAsync()).CopyTo<OrderItem, Domain.Entities.OrderItem>();
            return result;
        }
        public override void Update(Domain.Entities.Order t)
        {
            base.Update(t);
            context.OrderItem.RemoveRange(context.OrderItem.Where(x => t.OrderItems.Select(y => y.Id).Contains(x.Id)));
            context.OrderItem.AddRange(t.OrderItems.CopyTo<Domain.Entities.OrderItem, OrderItem>());
        }
    }
}
