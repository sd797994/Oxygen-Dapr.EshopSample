using Infrastructure.EfDataAccess;
using Infrastructure.PersistenceObject;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : RepositoryBase<EfDbContext, Domain.Entities.Account, Account>, Domain.Repository.IAccountRepository
    {
        private readonly EfDbContext context;
        public AccountRepository(EfDbContext context) : base(context) { this.context = context; }

        public override void Add(Domain.Entities.Account t)
        {
            var account = t.CopyTo<Domain.Entities.Account, Account>();
            context.Account.Add(account);
            var user = t.User.CopyTo<Domain.Entities.User, User>();
            user.AccountId = account.Id;
            if (account.Roles.Any())
                context.UserRole.AddRange(account.Roles.Select(x => new UserRole() { AccountId = account.Id, RoleId = x }));
            context.User.Add(user);
        }

        public async Task<bool> CheckAcccountUserInfoAny(Guid accountId)
        {
            return await context.User.AnyAsync(x => x.AccountId == accountId);
        }

        public async Task<Domain.Entities.Account> FindAccountByAccounId(string loginName)
        {
            var account = await context.Account.AsNoTracking().FirstOrDefaultAsync(x => x.LoginName == loginName);
            await FillAccountInfo(account);
            return account;
        }

        public async Task<List<string>> GetAccountPermissions(Guid accountId)
        {
            var roles = await (from userrole in context.UserRole.Where(x => x.AccountId == accountId)
                               join role in context.Role on userrole.RoleId equals role.Id
                               select role
                              ).AsNoTracking().ToListAsync();
            if (roles.Any(x => x.SuperAdmin))
                return null;
            return await (from rolepermission in context.RolePermission.Where(x => roles.Select(y => y.Id).Contains(x.RoleId))
                          join permission in context.Permission on rolepermission.PermissionId equals permission.Id
                          select permission.Path
                           ).AsNoTracking().ToListAsync();
        }

        public override async Task<Domain.Entities.Account> GetAsync(object key)
        {
            var account = await base.GetAsync(key);
            await FillAccountInfo(account);
            return account;
        }
        public override void Update(Domain.Entities.Account t)
        {
            var account = t.CopyTo<Domain.Entities.Account, Account>();
            context.Set<Account>().Attach(account).State = EntityState.Modified;
            var user = t.User.CopyTo<Domain.Entities.User, User>();
            user.AccountId = account.Id;
            if (context.User.Any(x => x.AccountId == account.Id))
                context.User.Update(user);
            else
                context.User.Add(user);
            if (account.Roles.Any())
            {
                context.UserRole.RemoveRange(context.UserRole.Where(x => x.AccountId == account.Id));
                context.UserRole.AddRange(account.Roles.Select(x => new UserRole() { AccountId = account.Id, RoleId = x }));
            }
        }
        async Task FillAccountInfo(Domain.Entities.Account account)
        {
            if (account != null)
            {
                account.User = await context.User.AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == account.Id) ?? new User();
                account.Roles = await context.UserRole.AsNoTracking().Where(x => x.AccountId == account.Id).Select(x => x.RoleId).ToListAsync() ?? new List<Guid>();
            }
        }
    }
}
