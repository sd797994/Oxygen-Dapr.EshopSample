using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Output
{
    public class GetGoodsListResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImage { get; set; }
        public int Stock { get; set; }
        public bool ShelfState { get; set; }
        public decimal Price { get; set; }
    }
}
