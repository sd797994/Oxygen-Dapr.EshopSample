using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class GetAccountUserNameByIdsResponse
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
    }
}
