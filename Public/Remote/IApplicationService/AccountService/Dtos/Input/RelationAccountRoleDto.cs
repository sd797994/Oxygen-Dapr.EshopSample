using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class RelationAccountRoleDto
    {
        [Required(ErrorMessage = "账户编号不能为空")]
        public Guid ID { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public List<Guid> Roles { get; set; }
    }
}
