using DomainBase;
using System;

namespace Domain
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public class LimitedTimeActivitie : Entity, IAggregateRoot
    {
        /// <summary>
        /// 活动名
        /// </summary>
        public string ActivitieName { get; set; }
        /// <summary>
        /// 促销商品
        /// </summary>
        public Guid GoodsId { get; set; }
        /// <summary>
        /// 促销价
        /// </summary>
        public decimal ActivitiePrice { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 活动是否上架
        /// </summary>
        public bool ShelfState { get; set; }

        public void CreateOrUpdate(string activitieName, Guid goodsId, decimal activitiePrice, DateTime startTime, DateTime endTime)
        {
            if (!string.IsNullOrEmpty(activitieName))
                ActivitieName = activitieName;
            GoodsId = goodsId;
            if (activitiePrice <= 0)
                throw new DomainException("活动价不能小于0");
            ActivitiePrice = activitiePrice;
            StartTime = startTime;
            if (endTime < DateTime.Now)
                throw new DomainException("活动结束时间不能小于当前时间");
            if (endTime > DateTime.Now.AddMonths(1))
                throw new DomainException("活动结束时间不能超过当前时间一个月");
            EndTime = endTime;
            if (startTime >= endTime)
                throw new DomainException("活动结束时间必须大于开始时间");
        }

        public void UpOrDownShelf(bool shelfState)
        {
            ShelfState = shelfState;
        }
    }
}
