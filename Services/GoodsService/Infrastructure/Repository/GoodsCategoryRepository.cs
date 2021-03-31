using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GoodsCategoryRepository : RepositoryBase<EfDbContext, Domain.Entities.GoodsCategory, GoodsCategory>, Domain.Repository.IGoodsCategoryRepository
    {
        private readonly EfDbContext context;
        public GoodsCategoryRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
