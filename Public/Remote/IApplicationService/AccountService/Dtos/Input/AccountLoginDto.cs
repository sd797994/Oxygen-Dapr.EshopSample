using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos
{
    public class AccountLoginDto
    {
        [Required(ErrorMessage = "请填写登录账号")]
        [RegularExpression("(?=.*[a-zA-Z0-9]).{6,12}", ErrorMessage = "账号长度在6-12位且只能是数字或字母")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "请填写密码")]
        [RegularExpression("(?=.*[0-9])(?=.*[a-zA-Z]).{8,20}", ErrorMessage = "密码长度在8-20位且必须包含数字+字母")]
        public string Password { get; set; }
        public bool LoginAdmin { get; set; }
    }
}
