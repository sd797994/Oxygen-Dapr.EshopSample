using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class SupplementaryUserDto
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "请填写真实姓名")]
        [RegularExpression("(?=.*[\u4e00-\u9fa5]).{2,4}", ErrorMessage = "真实姓名必须是2-4个汉字")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }
        
        /// <summary>
        /// 用户性别
        /// </summary>
        [Required(ErrorMessage = "请选择性别")]
        [Range(0, 2, ErrorMessage = "性别只能是男/女/未知")]
        public int Gender { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01 00:00:00", "9999-12-31 23:59:59", ErrorMessage = "出生年月格式不正确")]
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        [Required(ErrorMessage = "请填写收货地址")]
        public string Address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Required(ErrorMessage = "请填写联系电话")]
        [RegularExpression(@"(^(\d{3,4}-)?\d{6,8}$)|(^1[3-8]\d{9}$|^\d{3}-\d{8}$|^\d{4}-\d{7}$)", ErrorMessage = "请输入正确的手机号或座机号")]
        public string Tel { get; set; }
    }
}
