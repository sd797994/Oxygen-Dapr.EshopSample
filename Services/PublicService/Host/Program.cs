using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host;
using Host.Modules;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using k8s.Util.Common;
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
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json");
Configuration = builder.Configuration;
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    //注入oxygen依赖
    builder.RegisterOxygenModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddHttpClient();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
AuthenticationHandler.RegisterAllFilter();
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