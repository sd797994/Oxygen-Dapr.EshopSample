using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class UpdateAccountDto
    {
        [Required(ErrorMessage = "账户编号不能为空")]
        public Guid ID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
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
