using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos
{
    public class AccountLoginResponse
    {
        public AccountLoginResponse(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
