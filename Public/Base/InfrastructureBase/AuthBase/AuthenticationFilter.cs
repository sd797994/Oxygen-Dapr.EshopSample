using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.AuthBase
{
    //鉴权特性
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthenticationFilter : Attribute
    {
        public bool CheckPermission { get; set; }
        public AuthenticationFilter(bool checkPermission = true)
        {
            CheckPermission = checkPermission;
        }
    }
}
