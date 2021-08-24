using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Dtos
{
    public class RoleBaseInitCheckCache : StateStore
    {
        public RoleBaseInitCheckCache(object data)
        {
            Key = $"DarpEshopRoleBaseInitCheck";
            this.Data = data;
        }
        public RoleBaseInitCheckCache()
        {
            Key = $"DarpEshopRoleBaseInitCheck";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
        public override int? ttlInSeconds { get; set; }
    }
}
