using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService.Dtos.Output
{
    public class MallSettingOutInfo
    {
        public MallSettingOutInfo() { }
        public MallSettingOutInfo(string deliverName,string deliverAddress)
        {
            DeliverName = deliverName;
            DeliverAddress = deliverAddress;
        }
        public string DeliverName { get; set; }
        public string DeliverAddress { get; set; }
    }
}
