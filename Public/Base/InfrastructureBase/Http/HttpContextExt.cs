using Autofac;
using InfrastructureBase.AuthBase;
using Microsoft.AspNetCore.Http;
using Oxygen.Common.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastructureBase.Http
{
    public class HttpContextExt
    {
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string RoutePath { get; set; }
        public ILifetimeScope RequestService { get; set; }
        public CurrentUser User { get; set; }
        public HttpResponse Response { get; set; }
        public bool GetAuthIgnore()
        {
            if (Headers.TryGetValue("Authignore", out string val))
                return val == "true";
            return false;
        }
        public string GetLoginToken()
        {
            if (Headers.TryGetValue("Authentication", out string val))
                return val;
            return null;
        }
        static internal AsyncLocal<HttpContextExt> LocalVal { get; set; } = new AsyncLocal<HttpContextExt>();
        public static HttpContextExt Current { get => LocalVal.Value ?? default(HttpContextExt); }
        public static void SetCurrent(OxygenHttpContextWapper wapper)
        {
            LocalVal.Value = new HttpContextExt();
            LocalVal.Value.Headers = wapper.Headers;
            LocalVal.Value.Cookies = wapper.Cookies;
            LocalVal.Value.RoutePath = wapper.RoutePath;
            LocalVal.Value.RequestService = wapper.RequestService;
            LocalVal.Value.Response = wapper.HttpResponse;
        }
        public static void SetUser(CurrentUser user)
        {
            LocalVal.Value.User = user;
        }
    }
}
