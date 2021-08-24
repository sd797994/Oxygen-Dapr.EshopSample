using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Dtos
{
    public class OauthStateStore : StateStore
    {
        public OauthStateStore()
        {
            Key = "GithubUser";
        }
        public OauthStateStore(object Data)
        {
            Key = "GithubUser";
            this.Data = Data;
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
        public override int? ttlInSeconds { get; set; }
    }
}
