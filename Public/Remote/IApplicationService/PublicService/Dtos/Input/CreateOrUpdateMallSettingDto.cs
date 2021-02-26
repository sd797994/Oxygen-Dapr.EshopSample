using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService.Dtos.Input
{
    public class CreateOrUpdateMallSettingDto
    {
        /// <summary>
        /// 寄件人姓名
        /// </summary>
        [Required(ErrorMessage = "请输入寄件人姓名")]
        public string DeliverName { get; set; }
        /// <summary>
        /// 寄件人地址
        /// </summary>
        [Required(ErrorMessage = "请输入寄件人地址")]
        public string DeliverAddress { get; set; }
    }
}
