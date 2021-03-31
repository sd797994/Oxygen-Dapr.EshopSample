using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class LogisticsDeliverDto
    {
        [Required(ErrorMessage = "请选择订单")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "请选择物流类型")]
        [Range(0, 5, ErrorMessage = "物流类型选择错误")]
        public int LogisticsType { get; set; }
        [Required(ErrorMessage = "请输入物流回执")]
        public string LogisticsNo { get; set; }
        public DateTime? DeliveTime { get; set; }
    }
}
