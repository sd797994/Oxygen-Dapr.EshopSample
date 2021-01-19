using InfrastructureBase.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PersistenceObject
{
    public class UserRole: PersistenceObjectBase
    {
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }
    }
}
