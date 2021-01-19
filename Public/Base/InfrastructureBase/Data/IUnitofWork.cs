using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EfDataAccess
{
    public interface IUnitofWork
    {
        bool Commit(IDbContextTransaction tran = null);
        Task<bool> CommitAsync(IDbContextTransaction tran = null);
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
