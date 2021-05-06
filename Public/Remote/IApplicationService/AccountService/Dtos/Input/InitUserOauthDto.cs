using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Input
{
    public class InitUserOauthDto
    {
        public string OauthData { get; set; }

        public class Github
        {
            public string login { get; set; }
            public string avatar_url { get; set; }
            public string name { get; set; }
        }
    }
}
