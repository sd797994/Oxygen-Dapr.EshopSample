using System;
using DomainBase;

namespace Domain.Entities
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public class MallSetting : Entity, IAggregateRoot
    {
        /// <summary>
        /// 商铺名
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 商铺一句话描述
        /// </summary>
        public string ShopDescription { get; set; }
        /// <summary>
        /// 商铺图标
        /// </summary>
        public string ShopIconUrl { get; set; }
        /// <summary>
        /// 通用公告
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string DeliverName { get; set; }
        /// <summary>
        /// 寄件人地址
        /// </summary>
        public string DeliverAddress { get; set; }
        public void CreateOrUpdate(string shopName, string shopDescription, string shopIconUrl, string notice, string deliverName, string deliverAddress)
        {
            this.ShopName = shopName;
            this.ShopDescription = shopDescription;
            this.ShopIconUrl = shopIconUrl;
            this.Notice = notice;
            this.DeliverName = deliverName;
            this.DeliverAddress = deliverAddress;
        }
    }
}
