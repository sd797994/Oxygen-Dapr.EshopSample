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
        /// 寄件人姓名
        /// </summary>
        public string DeliverName { get; set; }
        /// <summary>
        /// 寄件人地址
        /// </summary>
        public string DeliverAddress { get; set; }
        public void CreateOrUpdate(string deliverName, string deliverAddress)
        {
            this.DeliverName = deliverName;
            this.DeliverAddress = deliverAddress;
        }
    }
}
