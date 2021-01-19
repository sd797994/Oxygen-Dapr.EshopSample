using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class CreatePermissionDto
    {
        [Required(ErrorMessage = "服务名不能为空")]
        public string ServerName { get; set; }

        [Required(ErrorMessage = "权限名不能为空")]
        public string PermissionName { get; set; }

        [Required(ErrorMessage = "接口地址不能为空")]
        public string Path { get; set; }
    }
}
