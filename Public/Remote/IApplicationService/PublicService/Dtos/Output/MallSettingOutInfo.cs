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
        public MallSettingOutInfo(string shopName, string shopDescription, string shopIconUrl, string notice, string deliverName,string deliverAddress)
        {
            this.ShopName = shopName;
            this.ShopDescription = shopDescription;
            this.ShopIconUrl = shopIconUrl;
            this.Notice = notice;
            DeliverName = deliverName;
            DeliverAddress = deliverAddress;
        }
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopIconUrl { get; set; }
        public string Notice { get; set; }
        public string DeliverName { get; set; }
        public string DeliverAddress { get; set; }
    }
}
