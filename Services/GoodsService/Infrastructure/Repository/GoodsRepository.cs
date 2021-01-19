using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;

namespace Infrastructure.Repository
{
    public class GoodsRepository : RepositoryBase<EfDbContext, Domain.Goods, Goods>, Domain.Repository.IGoodsRepository
    {
        private readonly EfDbContext context;
        public GoodsRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
