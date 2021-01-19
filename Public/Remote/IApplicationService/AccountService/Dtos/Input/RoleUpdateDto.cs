using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class RoleUpdateDto : RoleCreateDto
    {
        [Required(ErrorMessage = "请填写角色ID")]
        public Guid RoleId { get; set; }
    }
}
