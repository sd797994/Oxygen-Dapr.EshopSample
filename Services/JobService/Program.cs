using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using JobService.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oxygen.IocModule;
using Oxygen.Mesh.Dapr;
using Oxygen.Server.Kestrel.Implements;
using System.IO;
using System.Threading.Tasks;

namespace JobService
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
                    webhostbuilder.StartOxygenServer<OxygenActorStartup>((config) => {
                        config.Port = 80;
                        config.PubSubCompentName = "pubsub";
                        config.StateStoreCompentName = "statestore";
                        config.TracingHeaders = "Authentication";
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
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(configure =>
                    {
                        configure.AddConfiguration(_configuration.GetSection("Logging"));
                        configure.AddConsole();
                    });
                    GlobalConfiguration.Configuration.UseRedisStorage(_configuration.GetSection("RedisConnection").Value);
                    services.AddHangfire(x => { });
                    services.AddHangfireServer();
                    services.AddAutofac();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
