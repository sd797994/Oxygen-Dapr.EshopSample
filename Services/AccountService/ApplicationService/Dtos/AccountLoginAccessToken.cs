using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Dtos
{
    public class AccountLoginAccessToken : StateStore
    {
        public AccountLoginAccessToken(string key, AccessTokenItem data)
        {
            Key = $"DarpEshopUserAccountAccessToken_{key}";
            this.Data = data;
        }
        public AccountLoginAccessToken(string key)
        {
            Key = $"DarpEshopUserAccountAccessToken_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
    public class AccessTokenItem
    {
        public AccessTokenItem() { }
        public AccessTokenItem(Guid id, bool loginAdmin) { Id = id; LoginAdmin = loginAdmin; }
        public Guid Id { get; set; }
        public bool LoginAdmin { get; set; }
    }
}
