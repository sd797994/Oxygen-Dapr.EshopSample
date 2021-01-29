using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using System;

namespace Infrastructure.Repository
{
    public class GoodsRepository : RepositoryBase<EfDbContext, Domain.Entities.Goods, Goods>, Domain.Repository.IGoodsRepository
    {
        private readonly EfDbContext context;
        public GoodsRepository(EfDbContext context) : base(context) { this.context = context; Key = Guid.NewGuid(); }
        public Guid Key { get; set; }
    }
}
