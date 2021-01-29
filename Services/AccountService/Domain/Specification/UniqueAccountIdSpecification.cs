using DomainBase;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Specification
{
    /// <summary>
    /// 用户ID唯一规约
    /// </summary>
    public class UniqueAccountIdSpecification : ISpecification<Account>
    {
        private readonly IAccountRepository accountRepository;

        public UniqueAccountIdSpecification(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<bool> IsSatisfiedBy(Account entity)
        {
            if (await accountRepository.FindAccountByAccounId(entity.LoginName) == null)
                return true;
            else
                throw new DomainException("账户必须唯一");
        }
    }
}
