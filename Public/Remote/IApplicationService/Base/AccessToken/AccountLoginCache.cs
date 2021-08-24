using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AccessToken
{
    public class AccountLoginCache : StateStore
    {
        public AccountLoginCache(Guid key, object data)
        {
            Key = $"DarpEshopUserAccountInfo_{key}";
            this.Data = data;
        }
        public AccountLoginCache(Guid key)
        {
            Key = $"DarpEshopUserAccountInfo_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
        public override int? ttlInSeconds { get; set; }
    }
}
