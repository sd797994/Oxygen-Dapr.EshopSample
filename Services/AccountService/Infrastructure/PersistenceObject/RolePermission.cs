using InfrastructureBase.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PersistenceObject
{
    public class RolePermission: PersistenceObjectBase
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
