using Autofac;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Data.Nest;
using Oxygen.Client.ServerSymbol.Events;
using System.Linq;

namespace Host.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Common.GetProjectAssembliesArray()).Where(x => !new[] { "Microsoft", "System" }.Any(y => x.AssemblyQualifiedName.Contains(y)))
                .AsImplementedInterfaces().Where(x => !(x is IEventHandler))
                .InstancePerLifetimeScope();
            //事件订阅器需要独立注册因为其接口相同
            Common.RegisterAllEventHandlerInAutofac(Common.GetProjectAssembliesArray(), builder);
            //注入其他基础设施依赖
            builder.RegisterGeneric(typeof(ElasticSearchRepository<>)).As(typeof(IElasticSearchRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitofWorkManager<EfDbContext>>().As<IUnitofWork>().InstancePerLifetimeScope();
        }
    }
}
