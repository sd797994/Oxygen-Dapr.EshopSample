using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class PermissionState : StateStore
    {
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
