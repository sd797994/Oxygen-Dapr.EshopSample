using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class GoodsCreateDto
    {
        [Required(ErrorMessage = "请输入商品名")]
        [MaxLength(20, ErrorMessage = "商品名不能超过20字")]
        public string GoodsName { get; set; }
        [Required(ErrorMessage = "商品图片不能为空")]
        public string GoodsImage { get; set; }
        [Required(ErrorMessage = "请输入商品价格")]
        [Range(0, double.MaxValue, ErrorMessage = "商品价格不能小于0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "请输入商品库存")]
        [Range(0, double.MaxValue, ErrorMessage = "商品库存不能小于0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "请选择商品分类")]
        public Guid CategoryId { get; set; }
    }
}
