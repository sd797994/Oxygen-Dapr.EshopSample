using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EfDataAccess
{
    public class UnitofWorkManager<TContext> : IUnitofWork where TContext : DbContext
    {
        private readonly TContext context;
        public UnitofWorkManager(TContext context)
        {
            this.context = context;
        }
        public bool Commit(IDbContextTransaction tran = null)
        {
            var result = context.SaveChanges() > -1;
            if (result && tran != null)
                tran.Commit();
            return result;
        }
        public async Task<bool> CommitAsync(IDbContextTransaction tran = null)
        {
            var result = (await context.SaveChangesAsync()) > -1;
            if (result && tran != null)
                await tran.CommitAsync();
            return result;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }
    }
}
