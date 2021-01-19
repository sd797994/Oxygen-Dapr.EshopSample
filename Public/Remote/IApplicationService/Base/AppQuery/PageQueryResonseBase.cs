using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AppQuery
{
    public class PageQueryResonseBase<T>
    {
        public PageQueryResonseBase(List<T> data,int total)
        {
            this.PageData = data;
            this.PageTotal = total;
        }
        public List<T> PageData { get; set; }
        public int PageTotal { get; set; }
    }
}
