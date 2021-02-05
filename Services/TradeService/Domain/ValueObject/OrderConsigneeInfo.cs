using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject
{
    /// <summary>
    /// 收件人信息
    /// </summary>
    public class OrderConsigneeInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
    }
}
