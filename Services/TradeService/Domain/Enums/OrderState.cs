using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderState
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Create = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        Pay = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        Success = 2,
        /// <summary>
        /// 已取消
        /// </summary>
        Cancel = -1
    }
}
