using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImageService.Modules;
using Infrastructure.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oxygen.IocModule;
using Oxygen.Mesh.Dapr;
using Oxygen.ProxyGenerator.Implements;
using Oxygen.Server.Kestrel.Implements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageService
{
    public class Program
    {
        private static IConfiguration _configuration { get; set; }
        static async Task Main(string[] args)
        {
            await CreateDefaultHost(args).Build().RunAsync();
        }

        static IHostBuilder CreateDefaultHost(string[] args) => new HostBuilder()
                .ConfigureWebHostDefaults(webhostbuilder => {
                    //注册成为oxygen服务节点
                    webhostbuilder.StartOxygenServer<OxygenStartup>((config) => {
                        config.Port = 80;
                        config.PubSubCompentName = "pubsub";
                        config.StateStoreCompentName = "statestore";
                        config.TracingHeaders = "Authentication";
                        config.UseStaticFiles = true;
                    });
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                    _configuration = config.Build();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    //注入oxygen依赖
                    builder.RegisterOxygenModule();
                    //注入业务依赖
                    builder.RegisterModule(new ServiceModule());
                    builder.RegisterType<ImageAppService.ImageAppService>().As<IApplicationService.Base.IImageAppService>().InstancePerLifetimeScope();
                })
                .ConfigureServices((context, services) =>
                {
                    //注册全局拦截器
                    LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
                    //注册鉴权拦截器
                    ImageAuthenticationHandler.RegisterAllFilter();
                    services.AddLogging(configure =>
                    {
                        configure.AddConfiguration(_configuration.GetSection("Logging"));
                        configure.AddConsole();
                    });
                    services.AddAutofac();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}