using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class AllPermissionResponse
    {
        public Guid Id { get; set; }
        public string PermissionName { get; set; }
        public IEnumerable<AllPermissionResponse> Child { get; set; }
    }
}
