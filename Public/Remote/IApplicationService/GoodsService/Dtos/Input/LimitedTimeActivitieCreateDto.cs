using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.LimitedTimeActivitieService.Dtos.Input
{
    public class LimitedTimeActivitieCreateDto
    {
        /// <summary>
        /// 活动名
        /// </summary>
        [Required(ErrorMessage = "活动名必须填写")]
        [MaxLength(40, ErrorMessage = "活动名称必须在40字以内")]
        public string ActivitieName { get; set; }
        /// <summary>
        /// 促销商品
        /// </summary>
        public Guid GoodsId { get; set; }
        /// <summary>
        /// 促销价
        /// </summary>
        [Required(ErrorMessage = "促销价格必须填写")]
        [Range(0, double.MaxValue, ErrorMessage = "促销价格必须大于0")]
        public decimal ActivitiePrice { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        [Required(ErrorMessage = "活动开始时间必须填写")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        [Required(ErrorMessage = "活动结束时间必须填写")]
        public DateTime EndTime { get; set; }
    }
}
