using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class EventHandleErrorInfoRepository : RepositoryBase<EfDbContext, Domain.Entities.EventHandleErrorInfo, EventHandleErrorInfo>, Domain.Repository.IEventHandleErrorInfoRepository
    {
        private readonly EfDbContext context;
        public EventHandleErrorInfoRepository(EfDbContext context) : base(context) { this.context = context; }

    }
}
