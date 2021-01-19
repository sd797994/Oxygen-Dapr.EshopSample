using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class GetPermissionListResponse
    {
        public Guid Id { get; set; }
        public string ServerName { get; set; }
        public string PermissionName { get; set; }
        public string Path { get; set; }
    }
}
