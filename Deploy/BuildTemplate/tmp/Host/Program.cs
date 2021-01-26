using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host.Modules;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oxygen.IocModule;
using Oxygen.ProxyGenerator.Implements;
using System.IO;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }
        static async Task Main(string[] args)
        {
            await CreateDefaultHost(args).Build().RunAsync();
        }

        static IHostBuilder CreateDefaultHost(string[] args) => new HostBuilder()
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
                    builder.RegisterType<UnitofWorkManager<EfDbContext>>().As<IUnitofWork>().InstancePerLifetimeScope();
                })
                .ConfigureServices((context, services) =>
                {
                    //注册成为oxygen服务节点
                    services.StartOxygenServer(config => {
                        config.Port = 80;
                        config.PubSubCompentName = "pubsub";
                        config.StateStoreCompentName = "statestore";
                        config.TracingHeaders = "Authentication";
                    });
                    //注册自定义HostService
                    services.AddHostedService<CustomerService>();
                    //注册全局拦截器
                    LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
                    //注册鉴权拦截器
                    LimitedTimeActivitieAuthenticationHandler.RegisterAllFilter();
                    services.AddLogging(configure =>
                    {
                        configure.AddConfiguration(_configuration.GetSection("Logging"));
                        configure.AddConsole();
                    });
                    services.AddDbContext<EfDbContext>(options => options.UseNpgsql(_configuration.GetSection("SqlConnectionString").Value));
                    services.AddAutofac();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
