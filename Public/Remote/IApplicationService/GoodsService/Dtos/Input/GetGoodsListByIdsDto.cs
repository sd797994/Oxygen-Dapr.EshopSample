using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.GoodsService.Dtos.Input
{
    public class GetGoodsListByIdsDto
    {
        public GetGoodsListByIdsDto() { }
        public GetGoodsListByIdsDto(IEnumerable<Guid> ids)
        {
            this.Ids = ids.ToList();
        }
        public List<Guid> Ids { get; set; }
    }
}
