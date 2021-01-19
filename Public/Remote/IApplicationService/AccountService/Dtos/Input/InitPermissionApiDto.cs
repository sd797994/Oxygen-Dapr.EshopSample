using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class InitPermissionApiEvent<T>
    {
        public InitPermissionApiEvent() { }
        public InitPermissionApiEvent(T serviceApis)
        {
            this.ServiceApis = serviceApis;
        }
        public T ServiceApis { get; set; }
    }
}
