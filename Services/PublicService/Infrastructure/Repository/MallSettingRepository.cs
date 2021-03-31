using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MallSettingRepository : RepositoryBase<EfDbContext, Domain.Entities.MallSetting, MallSetting>, Domain.Repository.IMallSettingRepository
    {
        private readonly EfDbContext context;
        public MallSettingRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
