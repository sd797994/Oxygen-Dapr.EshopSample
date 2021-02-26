using DomainBase;
using InfrastructureBase;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EfDataAccess
{
    public abstract class RepositoryBase<TContext, DomainModel, PersistenceObject> : IRepository<DomainModel> where TContext : DbContext where PersistenceObject : class where DomainModel : Entity
    {
        private readonly TContext context;
        public RepositoryBase(TContext context)
        {
            this.context = context;
        }
        public virtual void Add(DomainModel t)
        {
            context.Set<PersistenceObject>().Add(t.CopyTo<DomainModel, PersistenceObject>());
        }


        public virtual void Delete(DomainModel t)
        {
            context.Set<PersistenceObject>().Remove(t.CopyTo<DomainModel, PersistenceObject>());
        }

        public virtual void Delete(Expression<Func<DomainModel, bool>> condition)
        {
            context.Set<PersistenceObject>().RemoveRange(context.Set<PersistenceObject>().Where(condition.ReplaceParameter<DomainModel, PersistenceObject>()));
        }

        public virtual async Task<DomainModel> GetAsync(object key = null)
        {
            PersistenceObject po;
            if (key == null)
                po = await context.Set<PersistenceObject>().FirstOrDefaultAsync();
            else
                po = await context.Set<PersistenceObject>().FindAsync(key);
            if (po == null)
                return default;
            context.Set<PersistenceObject>().Attach(po).State = EntityState.Detached;
            return po.CopyTo<PersistenceObject, DomainModel>();
        }
        public virtual async IAsyncEnumerable<DomainModel> GetManyAsync(Expression<Func<DomainModel, bool>> condition)
        {
            var poResult = await context.Set<PersistenceObject>().Where(condition.ReplaceParameter<DomainModel, PersistenceObject>()).ToListAsync();
            if (poResult.Any())
            {
                foreach (var po in poResult)
                {
                    yield return po.CopyTo<PersistenceObject, DomainModel>();
                }
            }
        }
        public virtual async IAsyncEnumerable<DomainModel> GetManyAsync(Guid[] key)
        {
            var keys = key.ToList();
            var poResult = await context.Set<PersistenceObject>().Where(x => keys.Contains((x as Entity).Id)).ToListAsync();
            if (poResult.Any())
            {
                foreach (var po in poResult)
                {
                    yield return po.CopyTo<PersistenceObject, DomainModel>();
                }
            }
        }

        public virtual void Update(DomainModel t)
        {
            var po = t.CopyTo<DomainModel, PersistenceObject>();
            context.Set<PersistenceObject>().Attach(po).State = EntityState.Modified;
        }

        public virtual async Task<bool> AnyAsync(object key = null)
        {
            if (key != null)
                return await context.Set<PersistenceObject>().AnyAsync(x => (x as Entity).Id == (Guid)key);
            else
                return await context.Set<PersistenceObject>().AnyAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<DomainModel, bool>> condition)
        {
            return await context.Set<PersistenceObject>().Where(condition.ReplaceParameter<DomainModel, PersistenceObject>()).AnyAsync();
        }
    }
}
