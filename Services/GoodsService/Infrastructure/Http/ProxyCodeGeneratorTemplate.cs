using Microsoft.CodeAnalysis;
using Oxygen.Client.ServerSymbol;
using Oxygen.Common.Implements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure.Http
{
    public class ProxyCodeGeneratorTemplate
    {
        public static IEnumerable<(string sourceName, string sourceCode)> GetTemplate<TInterface, TImpl>()
        {
            //获取所有标记为remote的servie
            var remoteservice = ReflectionHelper.GetTypesByAttributes(typeof(TInterface), true, typeof(RemoteServiceAttribute));
            //获取所有标记为remote的method构造具体的delegate
            foreach (var x in remoteservice)
            {
                var implType = ReflectionHelper.GetTypeByInterface(typeof(TImpl), x);
                if (implType != null)
                {
                    var methods = new List<MethodInfo>();
                    ReflectionHelper.GetMethodByFilter(x, typeof(RemoteFuncAttribute)).ToList().ForEach(y =>
                    {
                        var funcAttr = ReflectionHelper.GetAttributeProperyiesByMethodInfo<RemoteFuncAttribute>(y);
                        //生成服务调用代理
                        if (funcAttr.FuncType == FuncType.Actor)
                        {
                            methods.Add(y);
                        }
                    });
                    if (methods.Any())
                    {
                        yield return ($"{implType.Name}Actor.cs", FullTemplate(x, implType, methods.ToArray()));
                    }
                }
            }
        }

        static string FullTemplate(Type intefaceType, Type implType, MethodInfo[] methods)
        {
            var baseAcotModelType = implType.BaseType.GetProperty("ActorData").PropertyType;
            var source = new StringBuilder();
            source.Append(@"using System;
using Autofac;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using Oxygen.Mesh.Dapr.Model;
using System.Threading.Tasks;
namespace Oxygen.Mesh.Dapr.ProxyImpl
{
    public interface " + intefaceType.Name + @"Actor : IActor
    {
        ");
            foreach (var method in methods)
            {
                source.AppendLine("        Task<" + method.ReturnType.GetGenericArguments()[0] + "> " + method.Name + "(" + (method.GetParameters().Any() ? (method.GetParameters()[0].ParameterType + " " + method.GetParameters()[0].Name) : "") + ");");
            }
            source.Append(@"
    }
    public class " + intefaceType.Name + @"ActorImpl : BasicActor<" + baseAcotModelType.FullName + @">, " + intefaceType.Name + @"Actor
    {
        private readonly " + intefaceType.FullName + @" _actorService;
        public static Func<ActorStateModel, ILifetimeScope, Task> ActorServiceSaveData = (model, scope) => (scope.Resolve<" + intefaceType.FullName + @">() as BaseActorService<" + baseAcotModelType.FullName + @">).SaveData(model as " + baseAcotModelType.FullName + @", scope);
        public " + intefaceType.Name + @"ActorImpl(ActorHost actorHost, ILifetimeScope lifetimeScope, " + intefaceType.FullName + @" _actorService) : base(actorHost, lifetimeScope)
        {
            this._actorService = _actorService;

        }");
            foreach (var method in methods)
            {
                source.Append(@"
        public async Task<" + method.ReturnType.GetGenericArguments()[0] + "> " + method.Name + "(" + (method.GetParameters().Any() ? (method.GetParameters()[0].ParameterType + " " + method.GetParameters()[0].Name) : "") + @")
        {
            (_actorService as BaseActorService<" + baseAcotModelType.FullName + @">).ActorData = this.ActorData;
            var result = await _actorService." + method.Name + @"(" + (method.GetParameters().Any() ? (method.GetParameters()[0].Name) : "") + @");
            this.ActorData = (_actorService as BaseActorService<" + baseAcotModelType.FullName + @">).ActorData;
            return result;
        }");
            }
            source.AppendLine(@"
        
    }
}");
            return source.ToString();
        }
    }
}
