using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class FoodsRepository : RepositoryBase<EfDbContext, Domain.Entities.Foods, Foods>, Domain.Repository.IFoodsRepository
    {
        private readonly EfDbContext context;
        public FoodsRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
