using Autofac;
using InfrastructureBase.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.AopFilter
{
    public class test
    {
        public async Task Get<T>(T input)
        {
            await Task.CompletedTask;
        }
    }
    public class AopFilterManager
    {
        static Dictionary<string, Func<object, Task>> BeforeFilters = new Dictionary<string, Func<object, Task>>();
        static Dictionary<string, Func<object, Task>> AfterFilters = new Dictionary<string, Func<object, Task>>();
        static MethodInfo beforeMethodInfo = typeof(IAopMethodFilter).GetMethod(nameof(IAopMethodFilter.OnMethodExecuting));
        static MethodInfo afterMethodInfo = typeof(IAopMethodFilter).GetMethod(nameof(IAopMethodFilter.OnMethodExecuted));
        public static void RegisterAllFilter()
        {
            //查询所有注册了IAopMethodFilter类型的服务
            var alltype = Common.GetAllMethodByAopFilter();
            foreach (var item in alltype)
            {
                //将filter func通过字典的方式保存起来
                if(item.attrInstance is IAopMethodFilter filterInstance)
                {
                    BeforeFilters.Add(item.path, GetTypeFunc(beforeMethodInfo, filterInstance));
                    AfterFilters.Add(item.path, GetTypeFunc(afterMethodInfo, filterInstance));
                }
            }
            Func<object, Task> GetTypeFunc(MethodInfo methodInfo, IAopMethodFilter instance) => (Func<object, Task>)methodInfo.CreateDelegate(typeof(Func<object, Task>), instance);
        }

        public static async Task ExcutingMethodFilter(object input)
        {
            if (BeforeFilters.TryGetValue(HttpContextExt.Current.RoutePath, out var beforefunc))
                await beforefunc(input);
        }
        public static async Task ExcutedMethodFilter(object result)
        {
            if (AfterFilters.TryGetValue(HttpContextExt.Current.RoutePath, out var afterfunc))
                await afterfunc(result);
        }
    }
}
