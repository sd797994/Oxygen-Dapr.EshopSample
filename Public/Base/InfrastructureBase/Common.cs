using Autofac;
using InfrastructureBase.AuthBase;
using Microsoft.Extensions.DependencyModel;
using Oxygen.Client.ServerSymbol;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    public class Common
    {
        public static string GetMD5SaltCode(string origin, params object[] salt)
        {
            using var md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(origin + string.Join("", salt)))).Replace("-", "");
        }
        static Lazy<Assembly[]> Assemblies = new Lazy<Assembly[]>(() =>
        {
            var result = new List<Assembly>();
            foreach (var lib in DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Type != "referenceassembly"))
            {
                try
                {
                    result.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name)));
                }
                catch (Exception)
                {
                    //ingore
                }
            }
            return result.ToArray();
        });
        public static Assembly[] GetProjectAssembliesArray()
        {
            return Assemblies.Value;
        }
        public static List<AuthenticationInfo> GetAllMethodByAuthenticationFilter()
        {
            var result = new List<AuthenticationInfo>();
            foreach (var assembly in GetProjectAssembliesArray())
            {
                foreach (var type in assembly.GetTypes().Where(x => !x.IsInterface))
                {
                    if (type.GetInterfaces().Any())
                    {
                        var interfaceType = type.GetInterfaces()?[0];
                        if (interfaceType != null)
                        {
                            var remotesrvAttr = interfaceType.GetCustomAttribute<RemoteServiceAttribute>();
                            foreach (var method in type.GetMethods())
                            {
                                var authenFilter = method.GetCustomAttribute<AuthenticationFilter>();
                                if (authenFilter != null)
                                {
                                    var remotesrvfuncAttr = interfaceType.GetRuntimeMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray()).GetCustomAttribute<RemoteFuncAttribute>();
                                    if (remotesrvfuncAttr.FuncType == FuncType.Invoke)
                                        result.Add(new AuthenticationInfo(remotesrvAttr?.ServerDescription, remotesrvfuncAttr?.FuncDescription, authenFilter.CheckPermission, $"/{remotesrvAttr?.ServerName ?? interfaceType.Name}/{method.Name}".ToLower()));
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static void RegisterAllEventHandlerInAutofac(Assembly[] assemblies, ContainerBuilder builder)
        {
            foreach(var assembly in assemblies)
            {
                foreach(var type in assembly.GetTypes().Where(x => !x.IsInterface && x.GetInterface(nameof(IEventHandler)) != null))
                {
                    builder.RegisterType(type).As(type).InstancePerLifetimeScope();
                }
            }
        }
    }
}
