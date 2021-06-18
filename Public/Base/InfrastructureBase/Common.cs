using Autofac;
using InfrastructureBase.AopFilter;
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
        public static List<(string path, Attribute attrInstance)> GetAllMethodByAopFilter()
        {
            Func<Type, bool> TypeCondition = type => !type.IsInterface && type.GetInterfaces().Any();
            Func<MethodInfo, bool> MethodCondition = method => method.GetCustomAttributes().Any(x => x.GetType().GetInterfaces().Any(y => y.Equals(typeof(IAopMethodFilter))));
            Func<Type, MethodInfo, (string path, Attribute attrInstance)> CreateAuthenticationInfo = (type, method) => ($"/{type.GetInterfaces()[0].GetCustomAttribute<RemoteServiceAttribute>()?.ServerName ?? type.Name}/{method.Name}".ToLower(), method.GetCustomAttributes().FirstOrDefault(x => x.GetType().GetInterfaces().Any(y => y.Equals(typeof(IAopMethodFilter)))));
            return CreateTByTypeMethod(TypeCondition, MethodCondition, CreateAuthenticationInfo);
        }
        public static List<AuthenticationInfo> GetAllMethodByAuthenticationFilter()
        {
            Func<Type, bool> TypeCondition = type => !type.IsInterface && type.GetInterfaces().Any();
            Func<MethodInfo, bool> MethodCondition = method => method.GetCustomAttribute<AuthenticationFilter>() != null;
            Func<Type, MethodInfo, AuthenticationInfo> CreateAuthenticationInfo = (type, method) =>
            {
                var interfaceType = type.GetInterfaces()?[0];
                var remotesrvAttr = interfaceType.GetCustomAttribute<RemoteServiceAttribute>();
                var authenFilter = method.GetCustomAttribute<AuthenticationFilter>();
                var remotesrvfuncAttr = interfaceType.GetRuntimeMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray()).GetCustomAttribute<RemoteFuncAttribute>();
                if (remotesrvfuncAttr.FuncType == FuncType.Invoke)
                    return new AuthenticationInfo(remotesrvAttr?.ServerDescription, remotesrvfuncAttr?.FuncDescription, authenFilter.CheckPermission, $"/{remotesrvAttr?.ServerName ?? interfaceType.Name}/{method.Name}".ToLower());
                return default;
            };
            return CreateTByTypeMethod(TypeCondition, MethodCondition, CreateAuthenticationInfo);
        }
        internal static List<T> CreateTByTypeMethod<T>(Func<Type, bool> typeCondition, Func<MethodInfo, bool> methodCondition, Func<Type, MethodInfo, T> Tcreater)
        {
            return GetProjectAssembliesArray().SelectMany(assembly => assembly.GetTypes().Where(type => typeCondition(type)).SelectMany(type => type.GetMethods().Where(method => methodCondition(method)).Select(method => Tcreater(type, method)).Distinct()
            )).ToList();
        }
        static string[] SystemAssemblyQualifiedName = new string[] { "Microsoft", "System" };
        public static bool IsSystemType(Type type, bool checkBaseType = true, bool checkInterfaces = true)
        {
            if (SystemAssemblyQualifiedName.Any(x => type.AssemblyQualifiedName.StartsWith(x)))
                return true;
            else
            {
                if (checkBaseType && type.BaseType != null && type.BaseType != typeof(object) && SystemAssemblyQualifiedName.Any(x => type.BaseType.AssemblyQualifiedName.StartsWith(x)))
                    return true;
                if (checkInterfaces && type.GetInterfaces().Any())
                    if (type.GetInterfaces().Any(i => SystemAssemblyQualifiedName.Any(x => i.AssemblyQualifiedName.StartsWith(x))))
                        return true;
            }
            return false;
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
        public static IEnumerable<dynamic> GetRemoteServicesInfo()
        {
            return GetLazyRemoteServicesInfo.Value;
        }
        static Lazy<IEnumerable<dynamic>> GetLazyRemoteServicesInfo = new Lazy<IEnumerable<dynamic>>(() => {
            var result = new List<dynamic>();
            foreach (var interfaceType in typeof(IApplicationService.Base.AppQuery.PageQueryInputBase).Assembly.GetTypes().Where(x => x.IsInterface && !x.GetInterfaces().Any(x => x.Name == "IActorService")))
            {
                var remotesrvAttr = interfaceType.GetCustomAttribute<RemoteServiceAttribute>();
                if (remotesrvAttr != null && remotesrvAttr is RemoteServiceAttribute svcattr)
                {
                    result.Add(new 
                    {
                        ServiceName = remotesrvAttr.HostName.ToLower(),
                        PathName = interfaceType.GetMethods().Where(method =>
                        {
                            var remotefuncAttr = method.GetCustomAttribute<RemoteFuncAttribute>();
                            return remotefuncAttr != null && remotefuncAttr is RemoteFuncAttribute funattr && funattr.FuncType == FuncType.Invoke;
                        }).Select(x => $"{remotesrvAttr.ServerName}/{x.Name}".ToLower()).ToList()
                    });
                }
            }
            return result.GroupBy(x => x.ServiceName).Select(x => new
            {
                ServiceName = x.Key,
                PathName = x.SelectMany(y => (IEnumerable<string>)y.PathName)
            });
        });
        public static IEnumerable<dynamic> GetEnumValue<T>() where T : Enum
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                yield return new { key = Enum.GetName(typeof(T), item), value = (int)item };
            }
        }
    }
}
