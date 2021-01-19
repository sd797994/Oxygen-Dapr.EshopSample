using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CreatePermissionTmpDto
    {
        public string ServerName { get; set; }
        public string PermissionName { get; set; }
        public string Path { get; set; }
    }
}
