using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.AuthBase
{
    public abstract class AuthenticationManager
    {
        public static List<AuthenticationInfo> AuthenticationMethods = new List<AuthenticationInfo>();
        public static void RegisterAllFilter()
        {
            AuthenticationMethods = Common.GetAllMethodByAuthenticationFilter();
        }
        public abstract Task AuthenticationCheck(string routePath);
    }
    public class AuthenticationInfo
    {
        public AuthenticationInfo()
        {

        }
        public AuthenticationInfo(string srvName, string funcName, bool checkPermission, string path)
        {
            SrvName = srvName;
            FuncName = funcName;
            CheckPermission = checkPermission;
            Path = path;
        }
        public string SrvName { get; set; }
        public string FuncName { get; set; }
        public bool CheckPermission { get; set; }
        public string Path { get; set; }
    }
}
