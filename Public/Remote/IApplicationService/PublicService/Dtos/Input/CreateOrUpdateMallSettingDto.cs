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
        /// 商铺名
        /// </summary>
        [Required(ErrorMessage = "请输入商铺名")]
        public string ShopName { get; set; }
        /// <summary>
        /// 商铺一句话描述
        /// </summary>
        [Required(ErrorMessage = "请输入商铺一句话描述")]
        public string ShopDescription { get; set; }
        /// <summary>
        /// 商铺图标
        /// </summary>
        [Required(ErrorMessage = "请上传商铺图标")]
        public string ShopIconUrl { get; set; }
        /// <summary>
        /// 通用公告
        /// </summary>
        [Required(ErrorMessage = "请输入通用公告")]
        public string Notice { get; set; }
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
