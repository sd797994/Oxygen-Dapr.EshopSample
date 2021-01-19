using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class AccountDeleteDto
    {
        [Required(ErrorMessage = "账户编号不能为空")]
        public Guid AccountId { get; set; }
    }
}
