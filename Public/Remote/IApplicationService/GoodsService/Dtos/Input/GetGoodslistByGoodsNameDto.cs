using IApplicationService.Base.AppQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class GetGoodslistByGoodsNameDto: PageQueryInputBase
    {
        public string GoodsName { get; set; }
    }
}
