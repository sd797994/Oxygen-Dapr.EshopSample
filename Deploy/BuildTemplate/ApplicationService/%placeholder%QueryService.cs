using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.EfDataAccess;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;

namespace ApplicationService
{
    public class %placeholder%QueryService : IApplicationService.%placeholder%Service.%placeholder%QueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public %placeholder%QueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext, = dbContext, ;
            this.stateManager = stateManager;
        }
    }
}
