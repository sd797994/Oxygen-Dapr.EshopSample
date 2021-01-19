using IApplicationService.AccountService.Dtos.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IApplicationService.AccountService.Dtos
{
    public class CreateAccountDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请填写登录账号")]
        [RegularExpression("(?=.*[a-zA-Z0-9]).{6,12}", ErrorMessage = "账号长度在6-12位且只能是数字或字母")]
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请填写密码")]
        [RegularExpression("(?=.*[0-9])(?=.*[a-zA-Z]).{8,20}", ErrorMessage = "密码长度在8-20位且必须包含数字+字母")]
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "请填写昵称")]
        [RegularExpression(".{2,8}", ErrorMessage = "昵称长度在2-8位")]
        public string NickName { get; set; }

        public List<Guid> Roles { get; set; }
        public SupplementaryUserDto User { get; set; }
    }
}
