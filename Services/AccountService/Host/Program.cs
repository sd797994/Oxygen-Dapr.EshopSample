using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host;
using Host.Modules;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase.AopFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oxygen.IocModule;
using Oxygen.Mesh.Dapr;
using Oxygen.ProxyGenerator.Implements;
using Oxygen.Server.Kestrel.Implements;
using Oxygen.MulitlevelCache;
using Infrastructure.Cached;

IConfiguration Configuration = default;
var builder = OxygenApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseCors = true;
});
OxygenActorStartup.ConfigureServices(builder.Services);
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json");
    Configuration = config.Build();
}).ConfigureContainer<ContainerBuilder>(builder =>
{
    //注入oxygen依赖
    builder.RegisterOxygenModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.InjectionCached<L1Cache, L2Cache>();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
AccountAuthenticationHandler.RegisterAllFilter();
//注册自定义Attribute AOP拦截器(需要注册全局拦截器才有效)
AopFilterManager.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(Configuration.GetSection("SqlConnectionString").Value));
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
OxygenActorStartup.Configure(app, app.Services);
await app.RunAsync();