using DomainBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : Entity, IAggregateRoot
    {
        public string RoleName { get; set; }
        public bool SuperAdmin { get; set; }

        [NotMapped]
        public List<Guid> Permissions { get; set; }

        public void SetRole(string roleName, bool superAdmin, List<Guid>? permissions = null)
        {
            if (!string.IsNullOrEmpty(roleName))
                RoleName = roleName;
            SuperAdmin = superAdmin;
            if (!SuperAdmin && (permissions == null || !permissions.Any()))
                throw new DomainException("非超管角色请选择至少一个权限!");
            Permissions = permissions ?? new List<Guid>();
        }
    }
}
