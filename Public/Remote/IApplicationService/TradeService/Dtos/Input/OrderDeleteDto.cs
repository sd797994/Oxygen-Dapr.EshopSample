using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.TradeService.Dtos.Input
{
    public class OrderDeleteDto
    {
        [Required(ErrorMessage = "«Î—°‘Ò∂©µ•")]
        public Guid Id { get; set; }
    }
}
