using Autofac;
using InfrastructureBase;

namespace OauthService.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Common.GetProjectAssembliesArray()).Where(x => !Common.IsSystemType(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
