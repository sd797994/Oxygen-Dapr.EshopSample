using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "请输入商品类型")]
        [MaxLength(6, ErrorMessage = "商品类型名太长了")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "请输入排序")]
        [Range(0, int.MaxValue, ErrorMessage = "排序不能小于0")]
        public int Sort { get; set; }
    }
}
