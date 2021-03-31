using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repository
{
    public class GoodsRepository : RepositoryBase<EfDbContext, Domain.Entities.Goods, Goods>, Domain.Repository.IGoodsRepository
    {
        private readonly EfDbContext context;
        public GoodsRepository(EfDbContext context) : base(context) { this.context = context; }

        public override void Update(Domain.Entities.Goods t)
        {
            var po = t.CopyTo<Domain.Entities.Goods, Goods>();
            po.LastUpdateTime = DateTime.Now;
            context.Set<Goods>().Attach(po).State = EntityState.Modified;
        }
        public override void Add(Domain.Entities.Goods t)
        {
            var po = t.CopyTo<Domain.Entities.Goods, Goods>();
            po.LastUpdateTime = DateTime.Now;
            context.Set<Goods>().Add(po);
        }
    }
}
