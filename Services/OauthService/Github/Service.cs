using InfrastructureBase.Http;
using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OauthService.Github
{
    [RemoteService("oauthservice", "github", "github授权服务")]
    public interface IService
    {
        [RemoteFunc(funcDescription: "请求OAUTH登录")]
        Task<dynamic> GetUserInfo();
    }
    public class Service: IService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public Service(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<dynamic> GetUserInfo()
        {
            return await GetBaidu();
        }

        async Task<dynamic> GetGithub()
        {
            var model = new Model() { login = "" };
            if (HttpContextExt.Current.Headers.Any(x => x.Key.ToLower().Equals("myauth")))
            {
                var req = new HttpRequestMessage();
                req.Headers.Add("User-Agent", "dapr-eshop");
                req.Headers.Add("Authorization", HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key.ToLower().Equals("myauth")).Value);
                req.Method = HttpMethod.Get;
                req.RequestUri = new Uri("https://api.github.com/user");
                var result = await httpClientFactory.CreateClient().SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    HttpContextExt.Current.Response.Cookies.Append("githubuser", JsonSerializer.Serialize(JsonSerializer.Deserialize<Model>(content)),
                        new Microsoft.AspNetCore.Http.CookieOptions() { Domain = "dapreshop.com" });
                    HttpContextExt.Current.Response.Redirect("http://admin.dapreshop.com:30882");
                }
            }
            return model;
        }

        async Task<dynamic> GetBaidu()
        {
            var model = new Model() { login = "" };
            if (HttpContextExt.Current.Headers.Any(x => x.Key.ToLower().Equals("myauth")))
            {
                var requestUri = new Uri($"https://openapi.baidu.com/rest/2.0/passport/users/getLoggedInUser?access_token={HttpContextExt.Current.Headers.FirstOrDefault(x => x.Key.ToLower().Equals("myauth")).Value.Replace("Bearer ", "")}");
                var result = await httpClientFactory.CreateClient().GetAsync(requestUri);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    baidumodel obj = JsonSerializer.Deserialize<baidumodel>(content);
                    HttpContextExt.Current.Response.Cookies.Append("githubuser", JsonSerializer.Serialize(new Model() { login = obj.openid.Substring(0,8), name = obj.uname, avatar_url = $"http://tb.himg.baidu.com/sys/portraitn/item/{obj.portrait}" }),
                        new Microsoft.AspNetCore.Http.CookieOptions() { Domain = "dapreshop.com" });
                    HttpContextExt.Current.Response.Redirect("http://admin.dapreshop.com:30882");
                }
            }
            return model;
        }
    }
    public class baidumodel
    {
        public string uname { get; set; }
        public string portrait { get; set; }
        public string openid { get; set; }
    }
}
