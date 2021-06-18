using InfrastructureBase.AopFilter;
using InfrastructureBase.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodFilter : Attribute, IAopMethodFilter
    {
        public async Task OnMethodExecuting(object param)
        {
            Console.WriteLine($"我拦截了Before,请求到了路径:{HttpContextExt.Current.RoutePath}{(param == null ? "" : $", 请求参数:{JsonConvert.SerializeObject(param)}")}");
            await Task.CompletedTask;
        }
        public async Task OnMethodExecuted(object result)
        {
            Console.WriteLine($"我拦截了After,请求到了路径:{HttpContextExt.Current.RoutePath}{(result == null ? "" : $", 回调结果:{ JsonConvert.SerializeObject(result)}")}");
            await Task.CompletedTask;
        }

    }
}
