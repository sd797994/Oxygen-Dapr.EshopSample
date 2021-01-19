using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public partial class LoginAccountSuccessEvent: IEvent
    {
        public LoginAccountSuccessEvent()
        {

        }
        public LoginAccountSuccessEvent(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
