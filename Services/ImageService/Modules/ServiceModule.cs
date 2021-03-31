using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Runtime.Loader;
using InfrastructureBase;
using System.Linq;

namespace ImageService.Modules
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
