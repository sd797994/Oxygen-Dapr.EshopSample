using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class GetAccountListResponse
    {
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public int State { get; set; }

        public IEnumerable<RoleItem> Roles { get; set; }
        public class RoleItem
        {
            public Guid RoleId { get; set; }
            public string RoleName { get; set; }
            public bool SuperAdmin { get; set; }
        }
    }
}
