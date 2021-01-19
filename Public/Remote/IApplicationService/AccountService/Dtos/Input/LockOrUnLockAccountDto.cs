using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos
{
    public class LockOrUnLockAccountDto
    {

        [Required(ErrorMessage = "请填写账户编号")]
        public Guid ID { get; set; }
    }
}
