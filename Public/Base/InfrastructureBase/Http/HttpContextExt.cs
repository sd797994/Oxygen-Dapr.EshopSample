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
        public string RoutePath { get; set; }
        public ILifetimeScope RequestService { get; set; }
        public CurrentUser User { get; set; }
        public HttpContext HttpContext { get; set; }
        public bool GetAuthIgnore()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authignore", out var val))
                return val.Equals("true");
            return false;
        }
        public string GetLoginToken()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authentication", out var val))
                return val;
            return null;
        }
        static internal AsyncLocal<HttpContextExt> LocalVal { get; set; } = new AsyncLocal<HttpContextExt>();
        public static HttpContextExt Current { get => LocalVal.Value ?? default(HttpContextExt); }
        public static void SetCurrent(OxygenHttpContextWapper wapper)
        {
            LocalVal.Value = new HttpContextExt();
            LocalVal.Value.RoutePath = wapper.RoutePath;
            LocalVal.Value.RequestService = wapper.RequestService;
            LocalVal.Value.HttpContext = wapper.HttpContext;
        }
        public static void SetUser(CurrentUser user)
        {
            LocalVal.Value.User = user;
        }
    }
}
