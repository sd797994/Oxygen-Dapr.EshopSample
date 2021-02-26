using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EventHandleErrorInfo : Entity, IAggregateRoot
    {
        /// <summary>
        /// 订阅器名称
        /// </summary>
        public string HandlerName { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrMessage { get; set; }
        /// <summary>
        /// 异常堆栈
        /// </summary>
        public string ErrStackTrace { get; set; }
        /// <summary>
        /// 是否是系统异常
        /// </summary>
        public bool IsSystemErr { get; set; }
        /// <summary>
        /// 原始事件
        /// </summary>
        public string EventData { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}
