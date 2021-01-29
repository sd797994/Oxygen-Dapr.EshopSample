using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permission : Entity, IAggregateRoot
    {
        public Guid FatherId { get; set; }
        public string PermissionName { get; set; }
        public string Path { get; set; }

        public void CreatePermission(Guid fatherId, string permissionName, string path)
        {
            FatherId = fatherId;
            if (!string.IsNullOrEmpty(permissionName))
                PermissionName = permissionName;
            if (!string.IsNullOrEmpty(path))
                Path = path;
        }
    }
}
